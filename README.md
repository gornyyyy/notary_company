# Notary Company

Десктопное приложение для работы с нотариальной конторой.

## Требования

Перед установкой убедитесь, что у вас установлено следующее ПО:

1. **.NET SDK 10.0 или выше**  
   [Скачать .NET SDK](https://dotnet.microsoft.com/ru-ru/download/)

2. **Git**  
   [Скачать Git](https://git-scm.com/download/win)

3. **PostgreSQL 13–18 + pgAdmin 4**  
   (Важно: во время установки PostgreSQL запомните пароль пользователя `postgres`)

## Развёртывание приложения

### 1. Клонирование репозитория

Откройте **Командную строку** (или PowerShell) и выполните:

```bash
git clone https://github.com/gornyyyy/notary_company.git
cd notary_company
cd notary_company
```

### 2. Настройка подключения к базе данных

1. В корне проекта найдите файл appsettings.json
Откройте его и замените YOUR_PASSWORD_HERE на реальный пароль пользователя postgres:
```
{
  "ConnectionStrings": {
    "Postgres": "Host=localhost;Port=5432;Database=notary_company;Username=postgres;Password=YOUR_PASSWORD_HERE"
  }
}
```

### 3. Запуск приложения

В той же командной строке написать 
```
dotnet run
```

