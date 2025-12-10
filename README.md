# AntiPlagiarism

Проект реализует серверную часть системы проверки плагиата среди студентов

## Функционал:

1. Студент может загружать файл работы, указывая `studentId` и `assignmentId`
2. При этом работа сохраняется, фиксируется факт сдачи
3. Отдельный сервис анализирует работу и формирует отчёт о возможном плагиате
4. Преподаватель (или любой клиент) может запросить отчёты по работе
5. Для каждой работы строится облако слов по тексту файла через QuickChart Word Cloud API


## Архитектура системы

Система состоит из трёх микросервисов, общающихся по HTTP:

  1. `FileStoringService` — хранение работ:

  * Принимает файлы работ от gateway
  * Сохраняет файлы в файловой системе
  * Сохраняет метаданные в БД Postgres (`filestoringdb`)
  * Отдает метаданные и файлы по `workId`


  2. `FileAnalysisService` — анализ и отчёты:  
    Для работы `W`:  

      1. Скачивается файл `F(W)` из `FileStoringService`.
      2. Считается SHA-256 `H = SHA256(F(W))` -> ValueObject `FileContentHash`.
      3. В таблице `FileHashes` ищется самый ранний `FileHash` с тем же `Hash == H` и **другим `StudentId`**.
      4. Если такой найден:

        * `IsPlagiarism = true`,
        * `Similarity = 1.0`,
        * `SourceWorkId = найденный.WorkId`.  
          Иначе:

        * `IsPlagiarism = false`,
        * `Similarity = 0.0`,
        * `SourceWorkId = null`.
      5. Сохраняется запись `FileHash` для текущей работы.
      6. Извлекается текст работы (UTF-8, если не получилось — пустая строка).
      7. Генерируется `wordCloudUrl` через `IWordCloudGenerator`.
      8. Создаётся и сохраняется `Report`.




  3. `ApiGateway` — сервис-посредник для клиентов:
  * Является единой точкой входа для клиента
  * Оркестрирует сценарии загрузки работы в `FileStoringService` и запуска анализа в `FileAnalysisService`
  * Обрабатывает ошибки анализа, не ломая при этом сдачу работы (ошибки логируются)

  4. Логика взаимодействия:
  - Клиент -> **ApiGateway** -> `FileStoringService` (сохранение работы)
  - Клиент -> **ApiGateway** -> `FileAnalysisService` (запуск анализа / получение отчётов)
  - `FileAnalysisService` при необходимости обращается к `FileStoringService` за файлом по `workId`


## HTTP API:

### FileStoringService

* `POST /works` — приём работы (multipart/form-data):
  * поля: `file`, `studentId`, `assignmentId`
  * ответ: `201 Created` + `UploadWorkResponse`
* `GET /works/{id}` — метаданные работы
* `GET /works/{id}/file` — скачивание файла

---

### FileAnalysisService

* `POST /analysis/run` — запуск анализа:

  * тело:

    ```json
    {
      "workId": "guid",
      "studentId": "string",
      "assignmentId": "string"
    }
    ```
  * ответ:

    ```json
    {
      "reportId": "guid",
      "workId": "guid",
      "isPlagiarism": true,
      "sourceWorkId": "guid | null",
      "similarity": 1.0
    }
    ```
* `GET /analysis/works/{workId}/reports` — отчёты по работе:

  * ответ: массив

    ```json
    [
      {
        "reportId": "guid",
        "createdAt": "2025-12-10T00:00:00Z",
        "isPlagiarism": true,
        "sourceWorkId": "guid | null",
        "similarity": 1.0,
        "wordCloudUrl": "https://quickchart.io/wordcloud?text=..."
      }
    ]
    ```

---


## Сценарии работы:

### Сценарий 1. Студент загружает работу на проверку

1. Студент в интерфейсе выбирает файл работы и задание, к которому относится работа
2. Фронтенд отправляет запрос к API Gateway:

   `POST /api/works?studentId={studentId}&assignmentId={assignmentId}`  
   с телом `multipart/form-data`, содержащим `file` — загружаемый файл

3. API Gateway:
   1. Проверяет, что переданы `file`, `studentId`, `assignmentId`.
   2. Через FileStoringService:
      - пересылает файл и метаданные
      - получает `WorkId`, `StudentId`, `AssignmentId` (объект `WorkCreatedDto`)
   3. Через FileAnalysisService:
      - отправляет JSON `AnalysisDto { WorkId, StudentId, AssignmentId }`
      - пытается запустить анализ плагиата
      - при успехе получает `AnalysisResultDto`

4. API Gateway возвращает ответ:

   ```json
   {
     "workId": "<GUID работы>",
     "reportStatus": "Completed" | "AnalysisFailed",
     "report": { ... } | null
   }
      ```


---


### Сценарий 2. Преподаватель просматривает отчёты по конкретной работе

  1. Преподаватель в интерфейсе открывает работу студента
  2. Фронтенд отправляет запрос к API Gateway:
      `GET /api/works/{workId}/reports`
  3. API Gateway:  
    1. Отправляет запрос в FileAnalysisService:
      `GET /analysis/works/{workId}/reports`  
    2. Если FileAnalysisService возвращает неуспешный статус - логирует предупреждение и возвращает этот статус клиенту,
    если успешный - 
      - читает тело ответа (JSON с отчётами)
      - пробрасывает его клиенту как application/json



## Запуск и тестирование:

### Запуск через Docker

1. В корне решения выполнить:

```bash
docker compose up --build
```

2. Дождаться, пока все контейнеры поднимутся

Swagger UI:

* Gateway: `http://localhost:8080/swagger/index.html`
* FileStoringService: `http://localhost:5001/swagger/index.html`
* FileAnalysisService: `http://localhost:5002/swagger/index.html`

### 1. Сценарий без плагиата

1. Открыть `http://localhost:8080/swagger/index.html`
2. Выбрать `POST /api/works` → **Try it out**
3. Заполнить:

   * `studentId`: `student-1`
   * `assignmentId`: `kpo-hw3`
   * `file`: текстовый файл (UTF-8)
4. Нажать **Execute**

Ответ:

* статус `201 Created`;
* тело:

  ```json
  {
    "workId": "....",
    "reportStatus": "Completed",
    "report": {
      "reportId": "...",
      "workId": "...",
      "isPlagiarism": false,
      "sourceWorkId": null,
      "similarity": 0.0
    }
  }
  ```

5. Получить отчёты:

   * перейти к `GET /api/works/{workId}/reports`
   * подставить `workId` из ответа
   * нажать **Execute**

   Ответ: массив отчётов, у последнего

   * `isPlagiarism = false`
   * `similarity = 0.0`
   * `wordCloudUrl` — ссылка на облако слов

6. Открыть `wordCloudUrl` в новой вкладке — для просмотра картинки с облаком слов

### 2. Сценарий с плагиатом

1. Повторить `POST /api/works` с **тем же файлом**, но:

   * `studentId`: `student-2` (другой студент)
   * `assignmentId`: тот же (`kpo-hw3`)
2. В ответе получить новый `workId`
3. Вызвать `GET /api/works/{workId}/reports` для новой работы

Ожидаемый отчёт:

* `isPlagiarism = true`
* `similarity = 1.0`
* `sourceWorkId` — `workId` первой работы
* `wordCloudUrl` — ссылка на облако слов (по тексту второй работы)

### 3. Сценарий отказа анализа

1. В отдельном терминале остановить `fileanalysis-service`:

   ```bash
   docker compose stop fileanalysis-service
   ```

2. В Swagger gateway снова выполнить `POST /api/works`

3. Ответ:

   ```json
   {
     "workId": "...",
     "reportStatus": "AnalysisFailed",
     "report": null
   }
   ```

   То есть сдача работы прошла, а анализ не отработал — поведение контролируемое.

4. Запустить анализ снова:

   ```bash
   docker compose start fileanalysis-service
   ```

---
