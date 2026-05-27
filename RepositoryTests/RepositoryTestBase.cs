using Dapper;
using Microsoft.Data.Sqlite;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using notary_company.Models;
using notary_company.Repositories;
using System;
using System.Data;
using System.Linq;

namespace TestProj
{
    [TestClass]
    public abstract class RepositoryTestBase
    {
        protected IDbConnection Connection { get; private set; }

        [TestInitialize]
        public void TestInitialize()
        {
            var dbName = $"test_{Guid.NewGuid():N}";
            Connection = new SqliteConnection($"Data Source={dbName};Mode=Memory;Cache=Shared");
            Connection.Open();
            CreateSchema();
        }

        [TestCleanup]
        public void TestCleanup()
        {
            Connection?.Close();
            Connection?.Dispose();
        }

        private void CreateSchema()
        {
            Connection.Execute("PRAGMA foreign_keys = ON;");

            Connection.Execute(@"
                -- 1. Клиенты
                CREATE TABLE IF NOT EXISTS clients (
                    client_phone TEXT PRIMARY KEY,
                    client_name  TEXT NOT NULL
                );
 
                -- 2. Пользователи
                CREATE TABLE IF NOT EXISTS users (
                    user_id       INTEGER PRIMARY KEY AUTOINCREMENT,
                    login         TEXT NOT NULL UNIQUE,
                    password_hash TEXT NOT NULL
                );
 
                -- 3. Услуги
                CREATE TABLE IF NOT EXISTS services (
                    service_id          INTEGER PRIMARY KEY AUTOINCREMENT,
                    service_name        TEXT NOT NULL UNIQUE,
                    service_description TEXT,
                    service_price       REAL NOT NULL CHECK (service_price >= 0)
                );
 
                -- 4. Нотариусы и помощники
                --    BOOLEAN → INTEGER; FALSE → 0, TRUE → 1
                CREATE TABLE IF NOT EXISTS notaries (
                    notary_id          INTEGER PRIMARY KEY AUTOINCREMENT,
                    user_id            INTEGER NOT NULL UNIQUE
                                           REFERENCES users(user_id),
                    notary_name        TEXT    NOT NULL,
                    notary_description TEXT,
                    notary_phone       TEXT,
                    is_notary_helper   INTEGER NOT NULL DEFAULT 0
                );
 
                -- 5. Заявки
                --    request_status: CHECK воспроизводит PostgreSQL ENUM-подобное ограничение
                --    DATE → TEXT (формат 'YYYY-MM-DD'); DEFAULT CURRENT_DATE работает в SQLite
                CREATE TABLE IF NOT EXISTS requests (
                    request_id             INTEGER PRIMARY KEY AUTOINCREMENT,
                    client_phone           TEXT    NOT NULL
                                               REFERENCES clients(client_phone),
                    additional_information TEXT,
                    request_status         TEXT    NOT NULL DEFAULT 'ожидание'
                                               CHECK (request_status IN (
                                                   'ожидание',
                                                   'отказано',
                                                   'выполнено',
                                                   'назначена дата'
                                               )),
                    request_date           TEXT    NOT NULL DEFAULT (date('now')),
                    date_of_completion     TEXT
                );
 
                -- 6. Связующая таблица заявок и услуг
                CREATE TABLE IF NOT EXISTS request_services (
                    request_detail_id INTEGER PRIMARY KEY AUTOINCREMENT,
                    request_id        INTEGER NOT NULL REFERENCES requests(request_id),
                    service_id        INTEGER NOT NULL REFERENCES services(service_id),
                    UNIQUE (request_id, service_id)   -- unique_request_service
                );
            ");
        }
    }
}