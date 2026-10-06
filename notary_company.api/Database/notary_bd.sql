-- =====================================================
-- Создание таблиц для нотариальной конторы
-- =====================================================

-- 1. Таблица клиентов
CREATE TABLE clients (
    client_phone VARCHAR(20) PRIMARY KEY,
    client_name VARCHAR(100) NOT NULL
);

-- 2. Таблица заявок
CREATE TABLE requests (
    request_id SERIAL PRIMARY KEY,
    client_phone VARCHAR(20) NOT NULL REFERENCES clients(client_phone),
    total_approximate_price DECIMAL(10, 2) DEFAULT 0,
    additional_information TEXT,
    request_status VARCHAR(20) DEFAULT 'ожидание',
    request_date timestamp with time zone DEFAULT CURRENT_DATE,
    date_of_completion timestamp with time zone,
    
    CONSTRAINT check_request_status CHECK (request_status IN ('ожидание', 'отказано', 'выполнено', 'назначена дата'))
);

-- 3. Таблица услуг
CREATE TABLE services (
    service_id SERIAL PRIMARY KEY,
    service_name VARCHAR(100) NOT NULL UNIQUE,
    service_description TEXT,
    service_price DECIMAL(10, 2) NOT NULL CHECK (service_price >= 0)
);

-- 4. Связующая таблица заявок и услуг
CREATE TABLE request_services (
    request_detail_id SERIAL PRIMARY KEY,
    request_id INT NOT NULL REFERENCES requests(request_id),
    service_id INT NOT NULL REFERENCES services(service_id),
    
    CONSTRAINT unique_request_service UNIQUE (request_id, service_id)
);

-- 5. Таблица пользователей
CREATE TABLE users (
    user_id SERIAL PRIMARY KEY,
    login VARCHAR(50) NOT NULL UNIQUE,
    password_hash VARCHAR(255) NOT NULL
);

-- 6. Таблица нотариусов и помощников
CREATE TABLE notaries (
    notary_id SERIAL PRIMARY KEY,
    user_id INT NOT NULL UNIQUE REFERENCES users(user_id),
    notary_name VARCHAR(100) NOT NULL,
    notary_description TEXT,
    notary_phone VARCHAR(20),
    is_notary_helper BOOLEAN DEFAULT FALSE
);

-- =====================================================
-- Связи (внешние ключи)
-- =====================================================

-- Связь requests с clients
ALTER TABLE requests 
    ADD CONSTRAINT fk_requests_client 
    FOREIGN KEY (client_phone) REFERENCES clients(client_phone);

-- Связь request_services с requests
ALTER TABLE request_services 
    ADD CONSTRAINT fk_request_services_request 
    FOREIGN KEY (request_id) REFERENCES requests(request_id);

-- Связь request_services с services
ALTER TABLE request_services 
    ADD CONSTRAINT fk_request_services_service 
    FOREIGN KEY (service_id) REFERENCES services(service_id);

-- Связь notaries с users
ALTER TABLE notaries 
    ADD CONSTRAINT fk_notaries_user 
    FOREIGN KEY (user_id) REFERENCES users(user_id);