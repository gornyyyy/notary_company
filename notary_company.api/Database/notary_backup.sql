--
-- PostgreSQL database dump
--

\restrict 2n2mL4SHpKC887TMQnSOQYRJLa6YXEAnbyMU3yxL2z365o1AlVJiynucgYMfeWR

-- Dumped from database version 17.7
-- Dumped by pg_dump version 17.7

-- Started on 2026-05-31 23:07:09

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
-- TOC entry 217 (class 1259 OID 42080)
-- Name: clients; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.clients (
    client_phone character varying(20) NOT NULL,
    client_name character varying(100) NOT NULL
);


ALTER TABLE public.clients OWNER TO postgres;

--
-- TOC entry 227 (class 1259 OID 42144)
-- Name: notaries; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.notaries (
    notary_id integer NOT NULL,
    user_id integer NOT NULL,
    notary_name character varying(100) NOT NULL,
    notary_description text,
    notary_phone character varying(20),
    is_notary_helper boolean DEFAULT false
);


ALTER TABLE public.notaries OWNER TO postgres;

--
-- TOC entry 226 (class 1259 OID 42143)
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
-- TOC entry 4966 (class 0 OID 0)
-- Dependencies: 226
-- Name: notaries_notary_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--

ALTER SEQUENCE public.notaries_notary_id_seq OWNED BY public.notaries.notary_id;


--
-- TOC entry 223 (class 1259 OID 42116)
-- Name: request_services; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.request_services (
    request_detail_id integer NOT NULL,
    request_id integer NOT NULL,
    service_id integer NOT NULL
);


ALTER TABLE public.request_services OWNER TO postgres;

--
-- TOC entry 222 (class 1259 OID 42115)
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
-- TOC entry 4967 (class 0 OID 0)
-- Dependencies: 222
-- Name: request_services_request_detail_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--

ALTER SEQUENCE public.request_services_request_detail_id_seq OWNED BY public.request_services.request_detail_id;


--
-- TOC entry 219 (class 1259 OID 42086)
-- Name: requests; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.requests (
    request_id integer NOT NULL,
    client_phone character varying(20) NOT NULL,
    total_approximate_price numeric(10,2) DEFAULT 0,
    additional_information text,
    request_status character varying(20) DEFAULT 'ожидание'::character varying,
    request_date timestamp with time zone DEFAULT CURRENT_DATE,
    date_of_completion timestamp with time zone,
    CONSTRAINT check_request_status CHECK (((request_status)::text = ANY ((ARRAY['ожидание'::character varying, 'отказано'::character varying, 'выполнено'::character varying, 'назначена дата'::character varying])::text[])))
);


ALTER TABLE public.requests OWNER TO postgres;

--
-- TOC entry 218 (class 1259 OID 42085)
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
-- TOC entry 4968 (class 0 OID 0)
-- Dependencies: 218
-- Name: requests_request_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--

ALTER SEQUENCE public.requests_request_id_seq OWNED BY public.requests.request_id;


--
-- TOC entry 221 (class 1259 OID 42104)
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
-- TOC entry 220 (class 1259 OID 42103)
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
-- TOC entry 4969 (class 0 OID 0)
-- Dependencies: 220
-- Name: services_service_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--

ALTER SEQUENCE public.services_service_id_seq OWNED BY public.services.service_id;


--
-- TOC entry 225 (class 1259 OID 42135)
-- Name: users; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.users (
    user_id integer NOT NULL,
    login character varying(50) NOT NULL,
    password_hash character varying(255) NOT NULL
);


ALTER TABLE public.users OWNER TO postgres;

--
-- TOC entry 224 (class 1259 OID 42134)
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
-- TOC entry 4970 (class 0 OID 0)
-- Dependencies: 224
-- Name: users_user_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--

ALTER SEQUENCE public.users_user_id_seq OWNED BY public.users.user_id;


--
-- TOC entry 4773 (class 2604 OID 42180)
-- Name: notaries notary_id; Type: DEFAULT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.notaries ALTER COLUMN notary_id SET DEFAULT nextval('public.notaries_notary_id_seq'::regclass);


--
-- TOC entry 4771 (class 2604 OID 42181)
-- Name: request_services request_detail_id; Type: DEFAULT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.request_services ALTER COLUMN request_detail_id SET DEFAULT nextval('public.request_services_request_detail_id_seq'::regclass);


--
-- TOC entry 4766 (class 2604 OID 42182)
-- Name: requests request_id; Type: DEFAULT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.requests ALTER COLUMN request_id SET DEFAULT nextval('public.requests_request_id_seq'::regclass);


--
-- TOC entry 4770 (class 2604 OID 42183)
-- Name: services service_id; Type: DEFAULT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.services ALTER COLUMN service_id SET DEFAULT nextval('public.services_service_id_seq'::regclass);


--
-- TOC entry 4772 (class 2604 OID 42184)
-- Name: users user_id; Type: DEFAULT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.users ALTER COLUMN user_id SET DEFAULT nextval('public.users_user_id_seq'::regclass);


--
-- TOC entry 4950 (class 0 OID 42080)
-- Dependencies: 217
-- Data for Name: clients; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY public.clients (client_phone, client_name) FROM stdin;
89292920366	Прикольчиков Иван
87777779922	Карлсонов Михаил
89233334455	Филонов Федор
\.

-- TOC entry 4958 (class 0 OID 42135)
-- Dependencies: 225
-- Data for Name: users; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY public.users (user_id, login, password_hash) FROM stdin;
97	notary	$2a$11$BvWR4i7jccbfCqCD17iW1u3XvZ26A/8hx25X3yWknifTWctDgICfe
98	helper1	$2a$11$ZdYJ8psvQJvORUUpEZdkaOWbRI1k64pnOLRfet3UpDNnStbDp0qcO
\.

--
-- TOC entry 4960 (class 0 OID 42144)
-- Dependencies: 227
-- Data for Name: notaries; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY public.notaries (notary_id, user_id, notary_name, notary_description, notary_phone, is_notary_helper) FROM stdin;
45	97	Иванов Иван Иванович	Крутой нотариус(наверн)	8 999 999 88 77	f
46	98	Петров Петр Петрович	\N	\N	f
\.





--
-- TOC entry 4954 (class 0 OID 42104)
-- Dependencies: 221
-- Data for Name: services; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY public.services (service_id, service_name, service_description, service_price) FROM stdin;
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
--
-- TOC entry 4952 (class 0 OID 42086)
-- Dependencies: 219
-- Data for Name: requests; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY public.requests (request_id, client_phone, total_approximate_price, additional_information, request_status, request_date, date_of_completion) FROM stdin;
365	89292920366	0.00	Оооч надо срочно!!!	назначена дата	2026-05-20 02:01:11.957807+07	2026-05-16 16:00:00+07
366	87777779922	0.00	Срочно нужно оформить	отказано	2026-05-20 02:36:58.062975+07	\N
367	89233334455	0.00		выполнено	2026-05-20 03:03:17.031053+07	\N
\.

--
-- TOC entry 4956 (class 0 OID 42116)
-- Dependencies: 223
-- Data for Name: request_services; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY public.request_services (request_detail_id, request_id, service_id) FROM stdin;
8	365	55
9	365	57
10	366	67
11	367	54
\.

--
-- TOC entry 4971 (class 0 OID 0)
-- Dependencies: 226
-- Name: notaries_notary_id_seq; Type: SEQUENCE SET; Schema: public; Owner: postgres
--

SELECT pg_catalog.setval('public.notaries_notary_id_seq', 46, true);


--
-- TOC entry 4972 (class 0 OID 0)
-- Dependencies: 222
-- Name: request_services_request_detail_id_seq; Type: SEQUENCE SET; Schema: public; Owner: postgres
--

SELECT pg_catalog.setval('public.request_services_request_detail_id_seq', 11, true);


--
-- TOC entry 4973 (class 0 OID 0)
-- Dependencies: 218
-- Name: requests_request_id_seq; Type: SEQUENCE SET; Schema: public; Owner: postgres
--

SELECT pg_catalog.setval('public.requests_request_id_seq', 367, true);


--
-- TOC entry 4974 (class 0 OID 0)
-- Dependencies: 220
-- Name: services_service_id_seq; Type: SEQUENCE SET; Schema: public; Owner: postgres
--

SELECT pg_catalog.setval('public.services_service_id_seq', 68, true);


--
-- TOC entry 4975 (class 0 OID 0)
-- Dependencies: 224
-- Name: users_user_id_seq; Type: SEQUENCE SET; Schema: public; Owner: postgres
--

SELECT pg_catalog.setval('public.users_user_id_seq', 98, true);


--
-- TOC entry 4778 (class 2606 OID 42084)
-- Name: clients clients_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.clients
    ADD CONSTRAINT clients_pkey PRIMARY KEY (client_phone);


--
-- TOC entry 4794 (class 2606 OID 42152)
-- Name: notaries notaries_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.notaries
    ADD CONSTRAINT notaries_pkey PRIMARY KEY (notary_id);


--
-- TOC entry 4796 (class 2606 OID 42154)
-- Name: notaries notaries_user_id_key; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.notaries
    ADD CONSTRAINT notaries_user_id_key UNIQUE (user_id);


--
-- TOC entry 4786 (class 2606 OID 42121)
-- Name: request_services request_services_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.request_services
    ADD CONSTRAINT request_services_pkey PRIMARY KEY (request_detail_id);


--
-- TOC entry 4780 (class 2606 OID 42097)
-- Name: requests requests_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.requests
    ADD CONSTRAINT requests_pkey PRIMARY KEY (request_id);


--
-- TOC entry 4782 (class 2606 OID 42112)
-- Name: services services_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.services
    ADD CONSTRAINT services_pkey PRIMARY KEY (service_id);


--
-- TOC entry 4784 (class 2606 OID 42114)
-- Name: services services_service_name_key; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.services
    ADD CONSTRAINT services_service_name_key UNIQUE (service_name);


--
-- TOC entry 4788 (class 2606 OID 42123)
-- Name: request_services unique_request_service; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.request_services
    ADD CONSTRAINT unique_request_service UNIQUE (request_id, service_id);


--
-- TOC entry 4790 (class 2606 OID 42142)
-- Name: users users_login_key; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.users
    ADD CONSTRAINT users_login_key UNIQUE (login);


--
-- TOC entry 4792 (class 2606 OID 42140)
-- Name: users users_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.users
    ADD CONSTRAINT users_pkey PRIMARY KEY (user_id);


--
-- TOC entry 4803 (class 2606 OID 42175)
-- Name: notaries fk_notaries_user; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.notaries
    ADD CONSTRAINT fk_notaries_user FOREIGN KEY (user_id) REFERENCES public.users(user_id);


--
-- TOC entry 4799 (class 2606 OID 42165)
-- Name: request_services fk_request_services_request; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.request_services
    ADD CONSTRAINT fk_request_services_request FOREIGN KEY (request_id) REFERENCES public.requests(request_id);


--
-- TOC entry 4800 (class 2606 OID 42170)
-- Name: request_services fk_request_services_service; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.request_services
    ADD CONSTRAINT fk_request_services_service FOREIGN KEY (service_id) REFERENCES public.services(service_id);


--
-- TOC entry 4797 (class 2606 OID 42160)
-- Name: requests fk_requests_client; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.requests
    ADD CONSTRAINT fk_requests_client FOREIGN KEY (client_phone) REFERENCES public.clients(client_phone);


--
-- TOC entry 4804 (class 2606 OID 42155)
-- Name: notaries notaries_user_id_fkey; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.notaries
    ADD CONSTRAINT notaries_user_id_fkey FOREIGN KEY (user_id) REFERENCES public.users(user_id);


--
-- TOC entry 4801 (class 2606 OID 42124)
-- Name: request_services request_services_request_id_fkey; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.request_services
    ADD CONSTRAINT request_services_request_id_fkey FOREIGN KEY (request_id) REFERENCES public.requests(request_id);


--
-- TOC entry 4802 (class 2606 OID 42129)
-- Name: request_services request_services_service_id_fkey; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.request_services
    ADD CONSTRAINT request_services_service_id_fkey FOREIGN KEY (service_id) REFERENCES public.services(service_id);


--
-- TOC entry 4798 (class 2606 OID 42098)
-- Name: requests requests_client_phone_fkey; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.requests
    ADD CONSTRAINT requests_client_phone_fkey FOREIGN KEY (client_phone) REFERENCES public.clients(client_phone);


-- Completed on 2026-05-31 23:07:09

--
-- PostgreSQL database dump complete
--

\unrestrict 2n2mL4SHpKC887TMQnSOQYRJLa6YXEAnbyMU3yxL2z365o1AlVJiynucgYMfeWR

