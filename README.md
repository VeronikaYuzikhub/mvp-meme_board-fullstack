# MVP Full-Stack застосунок Meme Board
Навчальний проєкт з практики з конструювання ПЗ. Meme Board — це дошка мемів: можна зареєструватися, викладати картинки з підписом і категорією, дивитися стрічку, фільтрувати за категоріями і ставити лайки. Серверна частина — REST API на ASP.NET Core, клієнтська — окремий Vue-застосунок.

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
│   ├── Program.cs                  — точка входу, реєстрація сервісів і ендпоінтів
│   ├── ExceptionHandlingMiddleware.cs
│   ├── Models/                     — User, Meme, Category, MemeLike
│   ├── Data/AppDbContext.cs        — контекст БД (EF Core)І
│   ├── DTOs/                       — об'єкти для запитів і відповідей API
│   ├── Services/
│   │   ├── MemeLikeHelper.cs       — логіка лайків (для тестів)
│   │   └── MemeValidator.cs        — перевірка довжини title/description
│   ├── Migrations/                 — міграції SQLite
│   ├── backend.Tests/              — unit-тести
│   ├── appsettings.json            — рядок підключення, налаштування JWT
│   └── memeboard.db                — файл бази даних
│
└── frontend/
    ├── src/
    │   ├── main.js                 — вхід у застосунок
    │   ├── router/index.js         — маршрути
    │   ├── stores/auth.js          — стан авторизації
    │   ├── interceptors/http.js    — Axios + JWT у заголовку
    │   ├── composables/            — useMeme, useCategories тощо
    │   ├── components/             — Nav, MemeFeed, AddMeme, Sidebar...
    │   └── views/                  — HeroView, HomeView, AboutView...
    ├── index.html
    └── package.json
```

> Фронт у цьому проєкті — окремий Vue-проєкт у папці `frontend/`, а не статичні файли в `wwwroot`. Для роботи потрібно запускати бекенд і фронт у двох терміналах.

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

Preview зазвичai на `http://localhost:4173`. Бекенд при цьому теж має бути запущений, якщо потрібні запити до API.

**3. Тести**

```bash
cd backend
dotnet test
```

## Автор
Юзік Вероніка Євгенівна, група 3РП-10 — навчальна практика з конструювання програмного забезпечення
