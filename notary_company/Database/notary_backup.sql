--
-- PostgreSQL database dump
--

\restrict X3fFlfaKs8Me1986SPkY08f9dd05VK9gj9nKT6BBI3XHEZMTc6f71N0cQKRghJc

-- Dumped from database version 17.7
-- Dumped by pg_dump version 17.7

-- Started on 2026-05-31 22:35:44

SET statement_timeout = 0;
SET lock_timeout = 0;
SET idle_in_transaction_session_timeout = 0;
SET transaction_timeout = 0;
SET client_encoding = 'UTF8';
SET standard_conforming_strings = on;
SELECT pg_catalog.set_config('search_path', '', false);
SET check_function_bodies = false;
SET xmloption = content;
SET client_min_messages = warning;
SET row_security = off;

SET default_tablespace = '';

SET default_table_access_method = heap;

--
-- TOC entry 217 (class 1259 OID 41767)
-- Name: clients; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.clients (
    client_phone character varying(20) NOT NULL,
    client_name character varying(100) NOT NULL
);


ALTER TABLE public.clients OWNER TO postgres;

--
-- TOC entry 218 (class 1259 OID 41770)
-- Name: notaries; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.notaries (
    notary_id integer NOT NULL,
    user_id integer NOT NULL,
    notary_name character varying(100) NOT NULL,
    notary_description text,
    notary_phone character varying(20),
    is_notary_helper boolean DEFAULT true
);


ALTER TABLE public.notaries OWNER TO postgres;

--
-- TOC entry 219 (class 1259 OID 41776)
-- Name: notaries_notary_id_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

CREATE SEQUENCE public.notaries_notary_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER SEQUENCE public.notaries_notary_id_seq OWNER TO postgres;

--
-- TOC entry 4965 (class 0 OID 0)
-- Dependencies: 219
-- Name: notaries_notary_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--

ALTER SEQUENCE public.notaries_notary_id_seq OWNED BY public.notaries.notary_id;


--
-- TOC entry 220 (class 1259 OID 41777)
-- Name: request_services; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.request_services (
    request_detail_id integer NOT NULL,
    request_id integer NOT NULL,
    service_id integer NOT NULL
);


ALTER TABLE public.request_services OWNER TO postgres;

--
-- TOC entry 221 (class 1259 OID 41780)
-- Name: request_services_request_detail_id_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

CREATE SEQUENCE public.request_services_request_detail_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER SEQUENCE public.request_services_request_detail_id_seq OWNER TO postgres;

--
-- TOC entry 4966 (class 0 OID 0)
-- Dependencies: 221
-- Name: request_services_request_detail_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--

ALTER SEQUENCE public.request_services_request_detail_id_seq OWNED BY public.request_services.request_detail_id;


--
-- TOC entry 222 (class 1259 OID 41781)
-- Name: requests; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.requests (
    request_id integer NOT NULL,
    client_phone character varying(20) NOT NULL,
    additional_information text,
    request_status character varying(20) DEFAULT 'ожидание'::character varying,
    request_date timestamp with time zone DEFAULT CURRENT_DATE,
    date_of_completion timestamp with time zone,
    CONSTRAINT check_request_status CHECK (((request_status)::text = ANY (ARRAY[('ожидание'::character varying)::text, ('отказано'::character varying)::text, ('выполнено'::character varying)::text, ('назначена дата'::character varying)::text])))
);


ALTER TABLE public.requests OWNER TO postgres;

--
-- TOC entry 223 (class 1259 OID 41789)
-- Name: requests_request_id_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

CREATE SEQUENCE public.requests_request_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER SEQUENCE public.requests_request_id_seq OWNER TO postgres;

--
-- TOC entry 4967 (class 0 OID 0)
-- Dependencies: 223
-- Name: requests_request_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--

ALTER SEQUENCE public.requests_request_id_seq OWNED BY public.requests.request_id;


--
-- TOC entry 224 (class 1259 OID 41790)
-- Name: services; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.services (
    service_id integer NOT NULL,
    service_name character varying(100) NOT NULL,
    service_description text,
    service_price numeric(10,2) NOT NULL,
    CONSTRAINT services_service_price_check CHECK ((service_price >= (0)::numeric))
);


ALTER TABLE public.services OWNER TO postgres;

--
-- TOC entry 225 (class 1259 OID 41796)
-- Name: services_service_id_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

CREATE SEQUENCE public.services_service_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER SEQUENCE public.services_service_id_seq OWNER TO postgres;

--
-- TOC entry 4968 (class 0 OID 0)
-- Dependencies: 225
-- Name: services_service_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--

ALTER SEQUENCE public.services_service_id_seq OWNED BY public.services.service_id;


--
-- TOC entry 226 (class 1259 OID 41797)
-- Name: users; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.users (
    user_id integer NOT NULL,
    login character varying(50) NOT NULL,
    password_hash character varying(255) NOT NULL
);


ALTER TABLE public.users OWNER TO postgres;

--
-- TOC entry 227 (class 1259 OID 41800)
-- Name: users_user_id_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

CREATE SEQUENCE public.users_user_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER SEQUENCE public.users_user_id_seq OWNER TO postgres;

--
-- TOC entry 4969 (class 0 OID 0)
-- Dependencies: 227
-- Name: users_user_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--

ALTER SEQUENCE public.users_user_id_seq OWNED BY public.users.user_id;


--
-- TOC entry 4766 (class 2604 OID 41801)
-- Name: notaries notary_id; Type: DEFAULT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.notaries ALTER COLUMN notary_id SET DEFAULT nextval('public.notaries_notary_id_seq'::regclass);


--
-- TOC entry 4768 (class 2604 OID 41802)
-- Name: request_services request_detail_id; Type: DEFAULT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.request_services ALTER COLUMN request_detail_id SET DEFAULT nextval('public.request_services_request_detail_id_seq'::regclass);


--
-- TOC entry 4769 (class 2604 OID 41803)
-- Name: requests request_id; Type: DEFAULT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.requests ALTER COLUMN request_id SET DEFAULT nextval('public.requests_request_id_seq'::regclass);


--
-- TOC entry 4772 (class 2604 OID 41804)
-- Name: services service_id; Type: DEFAULT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.services ALTER COLUMN service_id SET DEFAULT nextval('public.services_service_id_seq'::regclass);


--
-- TOC entry 4773 (class 2604 OID 41805)
-- Name: users user_id; Type: DEFAULT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.users ALTER COLUMN user_id SET DEFAULT nextval('public.users_user_id_seq'::regclass);


--
-- TOC entry 4949 (class 0 OID 41767)
-- Dependencies: 217
-- Data for Name: clients; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY public.clients (client_phone, client_name) FROM stdin;
89292920366	Прикольчиков Иван
87777779922	Карлсонов Михаил
89233334455	Филонов Федор
\.


--
-- TOC entry 4950 (class 0 OID 41770)
-- Dependencies: 218
-- Data for Name: notaries; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY public.notaries (notary_id, user_id, notary_name, notary_description, notary_phone, is_notary_helper) FROM stdin;
43	97	Иванов Иван Иванович	Крутой нотариус (наверн)	8 923 329 99 88	f
44	98	Петров Петр Петрович	Помощничек №1	\N	t
\.


--
-- TOC entry 4952 (class 0 OID 41777)
-- Dependencies: 220
-- Data for Name: request_services; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY public.request_services (request_detail_id, request_id, service_id) FROM stdin;
\.


--
-- TOC entry 4954 (class 0 OID 41781)
-- Dependencies: 222
-- Data for Name: requests; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY public.requests (request_id, client_phone, additional_information, request_status, request_date, date_of_completion) FROM stdin;
365	89292920366	Оооч надо срочно!!!	назначена дата	2026-05-20 02:01:11.957807+07	2026-05-16 16:00:00+07
366	87777779922	Срочно нужно оформить	отказано	2026-05-20 02:36:58.062975+07	\N
367	89233334455		выполнено	2026-05-20 03:03:17.031053+07	\N
\.


--
-- TOC entry 4956 (class 0 OID 41790)
-- Dependencies: 224
-- Data for Name: services; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY public.services (service_id, service_name, service_description, service_price) FROM stdin;
77	Новое_97575413-b1bc-4db9-8c4d-fe7def3bab19	Старое описание	1000.00
53	Нотариальная доверенность	Письменное уполномочие, выдаваемое одним лицом (доверителем) другому лицу (поверенному) для представительства перед третьими лицами, удостоверенное нотариусом, подтверждающее законность делегированных полномочий	3800.00
54	Доверенность на квартиру	Официальный документ, предоставляющий поверенному право совершать сделки и юридические действия с конкретным объектом недвижимости: управление, распоряжение, регистрационные действия, получение документов и выписку из квартиры	555.00
55	Доверенность на автомобиль	Нотариально удостоверенный документ, дающий право поверенному управлять, распоряжаться транспортным средством, снимать и ставить на учёт в ГИБДД, проходить технический осмотр, оформлять страховку и получать документы на автомобиль	3800.00
56	Доверенность на ребёнка	Нотариальное уполномочие, выдаваемое родителями или законными представителями на сопровождение несовершеннолетнего ребёнка третьими лицами для поездок, посещения медицинских учреждений, образовательных организаций и совершения иных законных действий от имени ребёнка	3500.00
57	Договор купли-продажи	Соглашение сторон, удостоверенное нотариусом, по которому одна сторона (продавец) обязуется передать имущество в собственность другой стороне (покупателю), а покупатель обязуется принять имущество и уплатить за него определённую денежную сумму, с гарантией юридической чистоты сделки	14300.00
58	Договор дарения	Соглашение, по которому одна сторона (даритель) безвозмездно передаёт или обязуется передать имущество в собственность другой стороне (одаряемому), с подтверждением добровольности волеизъявления дарителя и отсутствия скрытых условий	14300.00
59	Брачный договор	Письменное соглашение супругов или лиц, вступающих в брак, удостоверенное нотариусом, определяющее имущественные права и обязанности супругов в браке и (или) в случае его расторжения, режим совместной, долевой или раздельной собственности на всё имущество или отдельные его виды	25800.00
60	Нотариальное согласие	Официальное удостоверенное нотариусом волеизъявление одного из супругов на совершение другим супругом сделки по распоряжению недвижимостью или иным имуществом, требующей нотариального удостоверения и (или) государственной регистрации, подтверждающее отсутствие возражений	26100.00
61	Согласие на продажу	Нотариально оформленный документ, выражающий одобрение законного владельца или собственника имущества (в том числе несовершеннолетнего, недееспособного, ограниченно дееспособного лица) на совершение сделки купли-продажи, дарения, мены или иного отчуждения объекта недвижимости	4300.00
62	Согласие на выезд	Нотариально удостоверенное разрешение законных представителей (родителей, опекунов, попечителей) на выезд несовершеннолетнего гражданина Российской Федерации за пределы страны, содержащее информацию о сроке выезда, странах посещения и сопровождающих лицах	3500.00
63	Вступление в наследство	Принятие заявления наследника о принятии наследства, проверку круга наследников по закону или завещанию, оценку наследственной массы, истребование документов, подтверждающих родство или право на наследство, с последующей выдачей свидетельства о праве на наследство	14300.00
64	Завещание на наследство	Нотариально удостоверенное письменное распоряжение гражданина о переходе принадлежащего ему имущества к определённым лицам или к государству на случай смерти, составленное в присутствии нотариуса, подтверждающее дееспособность завещателя и добровольность его волеизъявления	5700.00
65	Оформление наследства	Комплекс нотариальных услуг по ведению наследственного дела, включающий консультацию наследников, приём заявлений о принятии наследства или отказе от наследства, истребование необходимых документов из государственных органов, охрану наследственного имущества и управление им	14300.00
66	Свидетельство о наследстве	Официальный документ, выдаваемый наследникам, подтверждающий возникновение права собственности на наследственное имущество, содержащий перечень наследуемого имущества, его оценку, сведения о наследодателе и наследниках, доли наследников в общем наследстве	14300.00
67	Оформление квартиры	Полный цикл нотариальных услуг по регистрации права собственности на квартиру, включающий проверку юридической чистоты объекта, истребование правоустанавливающих документов, удостоверение сделки, подачу документов на государственную регистрацию права в Росреестр и получение выписки из ЕГРН	14300.00
68	Купли-продажа с материнским капиталом	Нотариальная сделка по приобретению жилого помещения с использованием средств материнского (семейного) капитала, включающая проверку целевого использования средств, оформление обязательства о выделении долей всем членам семьи, удостоверение договора купли-продажи и контроль распределения долей	11400.00
\.


--
-- TOC entry 4958 (class 0 OID 41797)
-- Dependencies: 226
-- Data for Name: users; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY public.users (user_id, login, password_hash) FROM stdin;
97	notary	$2a$11$BvWR4i7jccbfCqCD17iW1u3XvZ26A/8hx25X3yWknifTWctDgICfe
98	helper1	$2a$11$ZdYJ8psvQJvORUUpEZdkaOWbRI1k64pnOLRfet3UpDNnStbDp0qcO
\.


--
-- TOC entry 4970 (class 0 OID 0)
-- Dependencies: 219
-- Name: notaries_notary_id_seq; Type: SEQUENCE SET; Schema: public; Owner: postgres
--

SELECT pg_catalog.setval('public.notaries_notary_id_seq', 44, true);


--
-- TOC entry 4971 (class 0 OID 0)
-- Dependencies: 221
-- Name: request_services_request_detail_id_seq; Type: SEQUENCE SET; Schema: public; Owner: postgres
--

SELECT pg_catalog.setval('public.request_services_request_detail_id_seq', 7, true);


--
-- TOC entry 4972 (class 0 OID 0)
-- Dependencies: 223
-- Name: requests_request_id_seq; Type: SEQUENCE SET; Schema: public; Owner: postgres
--

SELECT pg_catalog.setval('public.requests_request_id_seq', 367, true);


--
-- TOC entry 4973 (class 0 OID 0)
-- Dependencies: 225
-- Name: services_service_id_seq; Type: SEQUENCE SET; Schema: public; Owner: postgres
--

SELECT pg_catalog.setval('public.services_service_id_seq', 68, true);


--
-- TOC entry 4974 (class 0 OID 0)
-- Dependencies: 227
-- Name: users_user_id_seq; Type: SEQUENCE SET; Schema: public; Owner: postgres
--

SELECT pg_catalog.setval('public.users_user_id_seq', 98, true);


--
-- TOC entry 4777 (class 2606 OID 41807)
-- Name: clients clients_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.clients
    ADD CONSTRAINT clients_pkey PRIMARY KEY (client_phone);


--
-- TOC entry 4779 (class 2606 OID 41809)
-- Name: notaries notaries_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.notaries
    ADD CONSTRAINT notaries_pkey PRIMARY KEY (notary_id);


--
-- TOC entry 4781 (class 2606 OID 41811)
-- Name: notaries notaries_user_id_key; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.notaries
    ADD CONSTRAINT notaries_user_id_key UNIQUE (user_id);


--
-- TOC entry 4783 (class 2606 OID 41813)
-- Name: request_services request_services_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.request_services
    ADD CONSTRAINT request_services_pkey PRIMARY KEY (request_detail_id);


--
-- TOC entry 4787 (class 2606 OID 41815)
-- Name: requests requests_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.requests
    ADD CONSTRAINT requests_pkey PRIMARY KEY (request_id);


--
-- TOC entry 4789 (class 2606 OID 41817)
-- Name: services services_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.services
    ADD CONSTRAINT services_pkey PRIMARY KEY (service_id);


--
-- TOC entry 4791 (class 2606 OID 41819)
-- Name: services services_service_name_key; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.services
    ADD CONSTRAINT services_service_name_key UNIQUE (service_name);


--
-- TOC entry 4785 (class 2606 OID 41821)
-- Name: request_services unique_request_service; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.request_services
    ADD CONSTRAINT unique_request_service UNIQUE (request_id, service_id);


--
-- TOC entry 4793 (class 2606 OID 41823)
-- Name: users users_login_key; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.users
    ADD CONSTRAINT users_login_key UNIQUE (login);


--
-- TOC entry 4795 (class 2606 OID 41825)
-- Name: users users_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.users
    ADD CONSTRAINT users_pkey PRIMARY KEY (user_id);


--
-- TOC entry 4796 (class 2606 OID 41826)
-- Name: notaries fk_notaries_user; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.notaries
    ADD CONSTRAINT fk_notaries_user FOREIGN KEY (user_id) REFERENCES public.users(user_id);


--
-- TOC entry 4798 (class 2606 OID 41831)
-- Name: request_services fk_request_services_request; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.request_services
    ADD CONSTRAINT fk_request_services_request FOREIGN KEY (request_id) REFERENCES public.requests(request_id);


--
-- TOC entry 4799 (class 2606 OID 41836)
-- Name: request_services fk_request_services_service; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.request_services
    ADD CONSTRAINT fk_request_services_service FOREIGN KEY (service_id) REFERENCES public.services(service_id);


--
-- TOC entry 4802 (class 2606 OID 41841)
-- Name: requests fk_requests_client; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.requests
    ADD CONSTRAINT fk_requests_client FOREIGN KEY (client_phone) REFERENCES public.clients(client_phone);


--
-- TOC entry 4797 (class 2606 OID 41846)
-- Name: notaries notaries_user_id_fkey; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.notaries
    ADD CONSTRAINT notaries_user_id_fkey FOREIGN KEY (user_id) REFERENCES public.users(user_id);


--
-- TOC entry 4800 (class 2606 OID 41851)
-- Name: request_services request_services_request_id_fkey; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.request_services
    ADD CONSTRAINT request_services_request_id_fkey FOREIGN KEY (request_id) REFERENCES public.requests(request_id);


--
-- TOC entry 4801 (class 2606 OID 41856)
-- Name: request_services request_services_service_id_fkey; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.request_services
    ADD CONSTRAINT request_services_service_id_fkey FOREIGN KEY (service_id) REFERENCES public.services(service_id);


--
-- TOC entry 4803 (class 2606 OID 41861)
-- Name: requests requests_client_phone_fkey; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.requests
    ADD CONSTRAINT requests_client_phone_fkey FOREIGN KEY (client_phone) REFERENCES public.clients(client_phone);


-- Completed on 2026-05-31 22:35:44

--
-- PostgreSQL database dump complete
--

\unrestrict X3fFlfaKs8Me1986SPkY08f9dd05VK9gj9nKT6BBI3XHEZMTc6f71N0cQKRghJc

