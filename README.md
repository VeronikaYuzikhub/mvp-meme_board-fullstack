# MVP Full-Stack застосунок Meme Board
Навчальний проєкт з практики з конструювання ПЗ. Meme Board - це дошка мемів: можна зареєструватися, викладати картинки з підписом і категорією, дивитися стрічку, фільтрувати за категоріями і ставити лайки. Серверна частина - REST API на ASP.NET Core, клієнтська - окремий Vue-застосунок.

## Опис
Застосунок реалізує облік основної сутності предметної області (Meme) з повним циклом CRUD, реєстрацією та авторизацією користувача за схемою OAuth 2.0 (JWT Bearer). Дані зберігаються в SQLite через Entity Framework Core. Картинки мемів передаються як base64 і зберігаються в базі разом із записом мема.

Front-End працює як SPA (Single Page Application): маршрути `/home`, `/about`, `/myMemes` тощо перемикаються через Vue Router без перезавантаження сторінки. Запити до API виконуються через Axios; після логіну JWT зберігається в `localStorage` під ключем `mvp_token`.

## Технологічний стек
**Back-End:** ASP.NET Core Minimal API, C#, Entity Framework Core, SQLite, Swagger (Swashbuckle), JWT Bearer, BCrypt (хеш паролів)

**Front-End:** Vue 3, Vue Router, Pinia, Axios, Vite, Bootstrap 5 (тема Bootswatch Zephyr), Font Awesome, AOS

**Тести:** xUnit (`backend.Tests`)

**Інше:** Git, Chrome DevTools

## Структура проєкту

```
mvp-meme_board-fullstack/
├── backend/
│   ├── Program.cs                  - точка входу, реєстрація сервісів і ендпоінтів
│   ├── ExceptionHandlingMiddleware.cs
│   ├── Models/                     - User, Meme, Category, MemeLike
│   ├── Data/AppDbContext.cs        - контекст БД (EF Core)І
│   ├── DTOs/                       - об'єкти для запитів і відповідей API
│   ├── Services/
│   │   ├── MemeLikeHelper.cs       - логіка лайків (для тестів)
│   │   └── MemeValidator.cs        - перевірка довжини title/description
│   ├── Migrations/                 - міграції SQLite
│   ├── backend.Tests/              - unit-тести
│   ├── appsettings.json            - рядок підключення, налаштування JWT
│   └── memeboard.db                - файл бази даних
│
└── frontend/
    ├── src/
    │   ├── main.js                 - вхід у застосунок
    │   ├── router/index.js         - маршрути
    │   ├── stores/auth.js          - стан авторизації
    │   ├── interceptors/http.js    - Axios + JWT у заголовку
    │   ├── composables/            - useMeme, useCategories тощо
    │   ├── components/             - Nav, MemeFeed, AddMeme, Sidebar...
    │   └── views/                  - HeroView, HomeView, AboutView...
    ├── index.html
    └── package.json
```

> Фронт у цьому проєкті - окремий Vue-проєкт у папці `frontend/`, а не статичні файли в `wwwroot`. Для роботи потрібно запускати бекенд і фронт у двох терміналах.

## Ендпоінти REST API

| Метод  | Шлях                 | Призначення                                      | Авторизація |
|--------|----------------------|--------------------------------------------------|-------------|
| POST   | /auth/register       | Реєстрація користувача                           | ні          |
| POST   | /auth/login          | Вхід, повертає `access_token` (JWT)              | ні          |
| GET    | /auth/me             | Профіль поточного користувача з claims токена    | так         |
| GET    | /categories          | Список категорій                                 | ні          |
| POST   | /categories          | Створити категорію                               | так         |
| GET    | /memes               | Список мемів (`?category=`, `?Title=`, `?mine=`) | ні*         |
| GET    | /memes/{id}          | Один мем за id                                   | ні          |
| POST   | /memes               | Створити мем                                     | так         |
| PUT    | /memes/{id}          | Редагувати свій мем                              | так         |
| DELETE | /memes/{id}          | Видалити свій мем                                | так         |
| POST   | /memes/{id}/like     | Поставити лайк                                   | так         |
| DELETE | /memes/{id}/like     | Прибрати лайк                                    | так         |
| POST   | /upload              | Завантажити зображення (jpeg/png → base64)       | так         |

\* параметр `mine=true` без токена поверне 401

Документація в Swagger: `http://localhost:5212/swagger`

## Запуск
Потрібні .NET SDK і Node.js. База SQLite створюється автоматично; міграції вже в репозиторії.

**1. Back-End**

```bash
cd backend
dotnet run
```

API: `http://localhost:5212`

**2. Front-End**

```bash
cd frontend
npm install
npm run dev
```

Сайт: `http://localhost:5173`

Production-збірка фронту (для перевірки Lighthouse):

```bash
cd frontend
npm run build:preview
```

Preview зазвичай на `http://localhost:4173`. Бекенд при цьому теж має бути запущений, якщо потрібні запити до API.

## Хостинг (production)
Проєкт можна подивитися не тільки локально - він також викладений в інтернет. Фронтенд (Vue) збирається через GitHub Actions і публікується на GitHub Pages (HTML, CSS і JS файли, без окремого сервера для фронту). Бекенд (ASP.NET API) працює на Render у Docker-контейнері і зберігає дані в SQLite-файлі `memeboard.db`.

Коли відкриваємо сайт у браузері, спочатку завантажується сторінка з GitHub Pages, а потім Vue сама звертається до API на Render за мемами, категоріями та авторизацією. URL бекенду підставляється під час збірки фронту (секрет `VITE_API_BASE_URL` у налаштуваннях репозиторію на GitHub).

**Посилання:**
- Сайт: https://veronikayuzikhub.github.io/mvp-meme_board-fullstack/
- API: https://mvp-meme-board-fullstack.onrender.com
- Swagger (документація API): https://mvp-meme-board-fullstack.onrender.com/swagger

**На що звернути увагу:** після push у гілку `main` деплой фронту на GitHub Pages зазвичай займає 1–2 хвилини - статус можна подивитися у вкладці Actions. Бекенд на Render безкоштовний і «засинає», якщо довго ніхто не заходить: перший запит після паузи може йти 30–60 секунд, тому стрічка мемів або категорії можуть трохи довше завантажуватися. Якщо все одно порожньо - варто оновити сторінку і почекати ще трохи.

**3. Тести**

```bash
cd backend
dotnet test
```

## Автор
Юзік Вероніка Євгенівна, група 3РП-10 - навчальна практика з конструювання програмного забезпечення

---

# MVP Full-Stack Application Meme Board
Educational project from software engineering practice. Meme Board is a meme board application: users can register, post images with a caption and category, browse the feed, filter by category, and like memes. The server side is a REST API built with ASP.NET Core; the client is a separate Vue application.

## Description
The application implements management of the core domain entity (Meme) with full CRUD operations, user registration, and authentication using OAuth 2.0 (JWT Bearer). Data is stored in SQLite via Entity Framework Core. Meme images are transmitted as base64 and stored in the database together with the meme record.

The front end runs as an SPA (Single Page Application): routes such as `/home`, `/about`, and `/myMemes` are handled by Vue Router without page reloads. API requests are made through Axios; after login, the JWT is stored in `localStorage` under the key `mvp_token`.

## Technology Stack

**Back-End:** ASP.NET Core Minimal API, C#, Entity Framework Core, SQLite, Swagger (Swashbuckle), JWT Bearer, BCrypt (password hashing)

**Front-End:** Vue 3, Vue Router, Pinia, Axios, Vite, Bootstrap 5 (Bootswatch Zephyr theme), Font Awesome, AOS

**Tests:** xUnit (`backend.Tests`)

**Other:** Git, Chrome DevTools

## Project Structure

```
mvp-meme_board-fullstack/
├── backend/
│   ├── Program.cs                  - entry point, service and endpoint registration
│   ├── ExceptionHandlingMiddleware.cs
│   ├── Models/                     - User, Meme, Category, MemeLike
│   ├── Data/AppDbContext.cs        - database context (EF Core)
│   ├── DTOs/                       - request and response objects for the API
│   ├── Services/
│   │   ├── MemeLikeHelper.cs       - like logic (for tests)
│   │   └── MemeValidator.cs        - title/description length validation
│   ├── Migrations/                 - SQLite migrations
│   ├── backend.Tests/              - unit tests
│   ├── appsettings.json            - connection string, JWT settings
│   └── memeboard.db                - database file
│
└── frontend/
    ├── src/
    │   ├── main.js                 - application entry point
    │   ├── router/index.js         - routes
    │   ├── stores/auth.js          - authentication state
    │   ├── interceptors/http.js    - Axios + JWT in request header
    │   ├── composables/            - useMeme, useCategories, etc.
    │   ├── components/             - Nav, MemeFeed, AddMeme, Sidebar...
    │   └── views/                  - HeroView, HomeView, AboutView...
    ├── index.html
    └── package.json
```

> In this project, the front end is a separate Vue application in the `frontend/` folder, not static files in `wwwroot`. Both the back end and front end must be running in separate terminals.

## REST API Endpoints

| Method | Path                 | Purpose                                          | Auth required |
|--------|----------------------|--------------------------------------------------|---------------|
| POST   | /auth/register       | User registration                                | no            |
| POST   | /auth/login          | Login, returns `access_token` (JWT)            | no            |
| GET    | /auth/me             | Current user profile from token claims           | yes           |
| GET    | /categories          | List of categories                               | no            |
| POST   | /categories          | Create a category                                | yes           |
| GET    | /memes               | List of memes (`?category=`, `?Title=`, `?mine=`) | no*          |
| GET    | /memes/{id}          | Single meme by id                                | no            |
| POST   | /memes               | Create a meme                                    | yes           |
| PUT    | /memes/{id}          | Edit own meme                                    | yes           |
| DELETE | /memes/{id}          | Delete own meme                                  | yes           |
| POST   | /memes/{id}/like     | Like a meme                                      | yes           |
| DELETE | /memes/{id}/like     | Remove like                                      | yes           |
| POST   | /upload              | Upload image (jpeg/png → base64)                 | yes           |

\* `mine=true` without a token returns 401

API documentation in Swagger: `http://localhost:5212/swagger`

## Running Locally

Requires .NET SDK and Node.js. The SQLite database is created automatically; migrations are included in the repository.

**1. Back-End**

```bash
cd backend
dotnet run
```

API: `http://localhost:5212`

**2. Front-End**

```bash
cd frontend
npm install
npm run dev
```

Application: `http://localhost:5173`

Production build of the front end (for Lighthouse testing):

```bash
cd frontend
npm run build:preview
```

Preview is usually available at `http://localhost:4173`. The back end must also be running if API requests are required.

## Hosting (production)

The application is deployed in a production environment. The front end (Vue) is built via GitHub Actions and published on GitHub Pages as static HTML, CSS, and JS files. The back end (ASP.NET REST API) runs on Render in a Docker container and stores data in the SQLite file `memeboard.db`.

When a user accesses the web interface, the page is served from GitHub Pages, and the Vue client sends HTTP requests to the API on Render for memes, categories, and authentication. The backend URL is injected at build time via the `VITE_API_BASE_URL` repository secret on GitHub.

Links:
- Web application: https://veronikayuzikhub.github.io/mvp-meme_board-fullstack/
- API: https://mvp-meme-board-fullstack.onrender.com
- Swagger: https://mvp-meme-board-fullstack.onrender.com/swagger

After a push to the `main` branch, the front end is redeployed automatically (typically 1–2 minutes; status is available in the Actions tab). On Render's free tier, the service may enter a sleep state after inactivity; the first request after idle time may take up to one minute.

**3. Tests**

```bash
cd backend
dotnet test
```

## Author

Veronika Yuzik, group 3RP-10 - educational practice in software engineering
