-- SCRIPT UNIFICADO: módulos de usuários, auditoria, estoque, produtos e vendas.
-- Os arquivos init - Pablo.sql e init - Henrique.sql são mantidos como reserva.

CREATE DATABASE IF NOT EXISTS Sistema;
USE Sistema;

-- USE ESSE TEMPLATE DE INIT PARA CRIAR AS TABELAS DO BANCO DE DADOS SÓ COPIE O INIT.SQL (EXEMPLO: init_pablo.sql) E COLOQUE SEU NOME,
-- VAMOS SEPARAR CADA INIT PARA DEPOIS AGRUPAR TODAS AS TABELAS EM UM SÓ ARQUIVO, PARA NÃO FICAR MUITO GRANDE E CONFUSO,
-- E PARA PODER SEPARAR OS MÓDULOS DO SISTEMA, COMO POR EXEMPLO: USUÁRIO, PRODUTO, PEDIDO, ETC.

CREATE TABLE IF NOT EXISTS `Users`(
	id BIGINT NOT NULL AUTO_INCREMENT,
	username VARCHAR(255) NOT NULL,
	hash CHAR(64) NOT NULL,
	created_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
	role ENUM('admin', 'seller', 'stocker', 'monetary', 'common') NOT NULL DEFAULT 'common',
	status BOOLEAN NOT NULL DEFAULT TRUE,
	active BOOLEAN NOT NULL DEFAULT FALSE,
	CONSTRAINT `Users_pkey` PRIMARY KEY (id),
	CONSTRAINT `Users_name_key` UNIQUE (username)
);

CREATE TABLE IF NOT EXISTS `logs`(
	id BIGINT NOT NULL AUTO_INCREMENT,
	username VARCHAR(255) NOT NULL,
	action VARCHAR(255) NULL,
	created_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
	CONSTRAINT `logs_pkey` PRIMARY KEY (id),
	CONSTRAINT `logs_username_fkey` FOREIGN KEY (username)
		REFERENCES `Users` (username)
		ON UPDATE CASCADE
);

-- Rotinas do módulo de usuários e auditoria.
DROP FUNCTION IF EXISTS `fn_hash_password`;
DROP PROCEDURE IF EXISTS `sp_create_user`;
DROP PROCEDURE IF EXISTS `sp_authenticate_user`;
DROP PROCEDURE IF EXISTS `sp_update_user`;
DROP PROCEDURE IF EXISTS `sp_change_password`;
DROP PROCEDURE IF EXISTS `sp_deactivate_user`;
DROP PROCEDURE IF EXISTS `sp_list_users`;
DROP PROCEDURE IF EXISTS `sp_register_log`;
DROP PROCEDURE IF EXISTS `sp_get_logs`;

DELIMITER $$

CREATE FUNCTION `fn_hash_password`(p_password VARCHAR(255))
RETURNS CHAR(64)
DETERMINISTIC
NO SQL
BEGIN
	RETURN SHA2(p_password, 256);
END$$

CREATE PROCEDURE `sp_create_user`(
	IN p_username VARCHAR(255),
	IN p_password VARCHAR(255),
	IN p_role VARCHAR(20)
)
BEGIN
	IF p_username IS NULL OR TRIM(p_username) = '' THEN
		SIGNAL SQLSTATE '45000' SET MESSAGE_TEXT = 'O nome de usuário é obrigatório.';
	END IF;

	IF p_password IS NULL OR p_password = '' THEN
		SIGNAL SQLSTATE '45000' SET MESSAGE_TEXT = 'A senha é obrigatória.';
	END IF;

	IF p_role IS NULL OR p_role NOT IN ('admin', 'seller', 'stocker', 'monetary', 'common') THEN
		SIGNAL SQLSTATE '45000' SET MESSAGE_TEXT = 'Perfil de usuário inválido.';
	END IF;

	IF EXISTS (SELECT 1 FROM `Users` WHERE username = p_username) THEN
		SIGNAL SQLSTATE '45000' SET MESSAGE_TEXT = 'Nome de usuário já cadastrado.';
	END IF;

	INSERT INTO `Users` (username, hash, role)
	VALUES (TRIM(p_username), fn_hash_password(p_password), p_role);

	SELECT LAST_INSERT_ID() AS id;
END$$

CREATE PROCEDURE `sp_authenticate_user`(
	IN p_username VARCHAR(255),
	IN p_password VARCHAR(255)
)
BEGIN
	SELECT id, username, role, status, active, created_at
	FROM `Users`
	WHERE username = p_username
	  AND hash = fn_hash_password(p_password)
	  AND status = TRUE
	  AND active = TRUE;
END$$

CREATE PROCEDURE `sp_update_user`(
	IN p_id BIGINT,
	IN p_username VARCHAR(255),
	IN p_role VARCHAR(20),
	IN p_status BOOLEAN,
	IN p_active BOOLEAN
)
BEGIN
	IF p_username IS NULL OR TRIM(p_username) = '' THEN
		SIGNAL SQLSTATE '45000' SET MESSAGE_TEXT = 'O nome de usuário é obrigatório.';
	END IF;

	IF p_role IS NULL OR p_role NOT IN ('admin', 'seller', 'stocker', 'monetary', 'common') THEN
		SIGNAL SQLSTATE '45000' SET MESSAGE_TEXT = 'Perfil de usuário inválido.';
	END IF;

	IF NOT EXISTS (SELECT 1 FROM `Users` WHERE id = p_id) THEN
		SIGNAL SQLSTATE '45000' SET MESSAGE_TEXT = 'Usuário não encontrado.';
	END IF;

	UPDATE `Users`
	SET username = TRIM(p_username),
		role = p_role,
		status = p_status,
		active = p_active
	WHERE id = p_id;
END$$

CREATE PROCEDURE `sp_change_password`(
	IN p_username VARCHAR(255),
	IN p_current_password VARCHAR(255),
	IN p_new_password VARCHAR(255)
)
BEGIN
	IF p_new_password IS NULL OR p_new_password = '' THEN
		SIGNAL SQLSTATE '45000' SET MESSAGE_TEXT = 'A nova senha é obrigatória.';
	END IF;

	IF NOT EXISTS (
		SELECT 1 FROM `Users`
		WHERE username = p_username
		  AND hash = fn_hash_password(p_current_password)
	) THEN
		SIGNAL SQLSTATE '45000' SET MESSAGE_TEXT = 'Senha atual inválida.';
	END IF;

	UPDATE `Users`
	SET hash = fn_hash_password(p_new_password)
	WHERE username = p_username;
END$$

CREATE PROCEDURE `sp_deactivate_user`(IN p_id BIGINT)
BEGIN
	IF NOT EXISTS (SELECT 1 FROM `Users` WHERE id = p_id) THEN
		SIGNAL SQLSTATE '45000' SET MESSAGE_TEXT = 'Usuário não encontrado.';
	END IF;

	UPDATE `Users`
	SET active = FALSE
	WHERE id = p_id;
END$$

CREATE PROCEDURE `sp_list_users`(IN p_only_active BOOLEAN)
BEGIN
	SELECT id, username, role, status, active, created_at
	FROM `Users`
	WHERE p_only_active = FALSE OR active = TRUE
	ORDER BY id;
END$$

CREATE PROCEDURE `sp_register_log`(
	IN p_username VARCHAR(255),
	IN p_action VARCHAR(255)
)
BEGIN
	IF p_username IS NULL OR TRIM(p_username) = '' THEN
		SIGNAL SQLSTATE '45000' SET MESSAGE_TEXT = 'O usuário do log é obrigatório.';
	END IF;

	IF p_action IS NULL OR TRIM(p_action) = '' THEN
		SIGNAL SQLSTATE '45000' SET MESSAGE_TEXT = 'A ação do log é obrigatória.';
	END IF;

	INSERT INTO `logs` (username, action)
	VALUES (TRIM(p_username), TRIM(p_action));

	SELECT LAST_INSERT_ID() AS id;
END$$

CREATE PROCEDURE `sp_get_logs`(
	IN p_username VARCHAR(255),
	IN p_limit INT
)
BEGIN
	IF p_limit IS NULL OR p_limit < 1 OR p_limit > 1000 THEN
		SET p_limit = 500;
	END IF;

	IF p_username IS NULL OR TRIM(p_username) = '' THEN
		SELECT id, username, action, created_at
		FROM `logs`
		ORDER BY created_at DESC
		LIMIT p_limit;
	ELSE
		SELECT id, username, action, created_at
		FROM `logs`
		WHERE username = p_username
		ORDER BY created_at DESC
		LIMIT p_limit;
	END IF;
END$$

DELIMITER ;


-- ==========================================================
-- 1. TABELAS BASE (CATEGORIAS, FORNECEDORES E CLIENTES)
-- ==========================================================

CREATE TABLE IF NOT EXISTS category (
    id_category BIGINT NOT NULL AUTO_INCREMENT,
    name_category VARCHAR(150) NOT NULL,
    PRIMARY KEY (id_category),
    UNIQUE KEY uk_category_name (name_category)
) ENGINE=InnoDB;

CREATE TABLE IF NOT EXISTS supplier (
    id_supplier BIGINT NOT NULL AUTO_INCREMENT,
    name VARCHAR(150) NOT NULL,
    document VARCHAR(20) NOT NULL,
    phone VARCHAR(20),
    is_active TINYINT(1) NOT NULL DEFAULT 1,
    created_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
    PRIMARY KEY (id_supplier),
    UNIQUE KEY uk_supplier_document (document),
    INDEX idx_supplier_name_active (is_active, name)
) ENGINE=InnoDB;

CREATE TABLE IF NOT EXISTS customer (
    id_customer BIGINT NOT NULL AUTO_INCREMENT,
    name VARCHAR(150) NOT NULL,
    nickname VARCHAR(150),
    cpf_cnpj VARCHAR(18),
    rg_ie VARCHAR(20),
    phone VARCHAR(15),
    email VARCHAR(150),
    cep VARCHAR(9),
    street VARCHAR(150),
    number_address VARCHAR(10),
    neighborhood VARCHAR(60),
    complement VARCHAR(60),
    city VARCHAR(80),
    uf CHAR(2),
    credit_limit DECIMAL(12,2) NOT NULL DEFAULT 0.00,
    is_active TINYINT(1) NOT NULL DEFAULT 1,
    notes TEXT,
    created_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
    updated_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
    PRIMARY KEY (id_customer),
    UNIQUE KEY uk_customer_cpf_cnpj (cpf_cnpj),
    INDEX idx_customer_name_active (is_active, name),
    INDEX idx_customer_nickname (nickname),
    INDEX idx_customer_phone (phone),
    CONSTRAINT chk_customer_credit_limit CHECK (credit_limit >= 0)
) ENGINE=InnoDB;

-- Cliente padrão
INSERT IGNORE INTO customer (id_customer, name, nickname, is_active)
VALUES (1, 'CONSUMIDOR FINAL', 'BALCAO', 1);

-- ==========================================================
-- 2. TABELA DE PRODUTOS
-- ==========================================================

CREATE TABLE IF NOT EXISTS product (
    id_product INT NOT NULL AUTO_INCREMENT,
    name VARCHAR(200) NOT NULL,
    description TEXT,
    purchase_price DECIMAL(12,2) NOT NULL DEFAULT 0.00,
    sale_price DECIMAL(12,2) NOT NULL DEFAULT 0.00,
    location VARCHAR(100),
    stock_quantity DECIMAL(12,3) NOT NULL DEFAULT 0.000,
    min_stock INT NOT NULL DEFAULT 0,
    unit VARCHAR(10) DEFAULT 'UN',
    image_url TEXT,
    is_active TINYINT(1) NOT NULL DEFAULT 1,
    updated_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
    id_category BIGINT,
    id_supplier BIGINT,
    PRIMARY KEY (id_product),
    INDEX idx_product_name_active (is_active, name),
    INDEX idx_product_location (location),
    INDEX idx_product_stock_alert (is_active, stock_quantity, min_stock),
    CONSTRAINT fk_product_category
        FOREIGN KEY (id_category) REFERENCES category (id_category)
        ON UPDATE CASCADE
        ON DELETE SET NULL,
    CONSTRAINT fk_product_supplier
        FOREIGN KEY (id_supplier) REFERENCES supplier (id_supplier)
        ON UPDATE CASCADE
        ON DELETE SET NULL,
    CONSTRAINT chk_product_prices CHECK (purchase_price >= 0 AND sale_price >= 0),
    CONSTRAINT chk_product_min_stock CHECK (min_stock >= 0)
) ENGINE=InnoDB;

-- ==========================================================
-- 3. TABELAS DE VENDAS, ITENS E PAGAMENTOS
-- ==========================================================

CREATE TABLE IF NOT EXISTS sale (
    id_sale BIGINT NOT NULL AUTO_INCREMENT,
    created_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
    sale_date TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
    subtotal DECIMAL(12,2) NOT NULL DEFAULT 0.00,
    discount DECIMAL(12,2) NOT NULL DEFAULT 0.00,
    total DECIMAL(12,2) NOT NULL DEFAULT 0.00,
    payment_status VARCHAR(30) NOT NULL DEFAULT 'PENDENTE',
    notes TEXT,
    id_client BIGINT NOT NULL DEFAULT 1,
    sale_type VARCHAR(30) NOT NULL DEFAULT 'VENDA',
    issued_fiscal_coupon TINYINT(1) NOT NULL DEFAULT 0,
    issued_promissory_note TINYINT(1) NOT NULL DEFAULT 0,
    seller_name VARCHAR(120),
    PRIMARY KEY (id_sale),
    INDEX idx_sale_date_type (sale_date, sale_type),
    INDEX idx_sale_status_date (payment_status, sale_date),
    INDEX idx_sale_client_date (id_client, sale_date),
    INDEX idx_sale_seller (seller_name),
    CONSTRAINT fk_sale_customer
        FOREIGN KEY (id_client) REFERENCES customer (id_customer)
        ON UPDATE CASCADE
        ON DELETE RESTRICT,
    CONSTRAINT chk_sale_values CHECK (subtotal >= 0 AND discount >= 0 AND total >= 0)
) ENGINE=InnoDB;

CREATE TABLE IF NOT EXISTS sale_product (
    id_sale_product BIGINT NOT NULL AUTO_INCREMENT,
    id_sale BIGINT NOT NULL,
    id_product INT NOT NULL,
    quantity DECIMAL(12,3) NOT NULL,
    unit_price DECIMAL(12,2) NOT NULL,
    unit_cost DECIMAL(12,2) NOT NULL,
    subtotal DECIMAL(12,2) NOT NULL,
    discount_product DECIMAL(12,2) NOT NULL DEFAULT 0.00,
    PRIMARY KEY (id_sale_product),
    INDEX idx_sale_product_sale_item (id_sale, id_product),
    CONSTRAINT fk_sale_product_sale
        FOREIGN KEY (id_sale) REFERENCES sale (id_sale)
        ON UPDATE CASCADE
        ON DELETE CASCADE,
    CONSTRAINT fk_sale_product_product
        FOREIGN KEY (id_product) REFERENCES product (id_product)
        ON UPDATE CASCADE
        ON DELETE RESTRICT,
    CONSTRAINT chk_sale_product_qty CHECK (quantity > 0),
    CONSTRAINT chk_sale_product_values CHECK (unit_price >= 0 AND unit_cost >= 0 AND subtotal >= 0 AND discount_product >= 0)
) ENGINE=InnoDB;

CREATE TABLE IF NOT EXISTS sale_payment (
    id_sale_payment BIGINT NOT NULL AUTO_INCREMENT,
    id_sale BIGINT NOT NULL,
    payment_method VARCHAR(50) NOT NULL,
    amount DECIMAL(10,2) NOT NULL,
    installments INT NOT NULL DEFAULT 1,
    created_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
    PRIMARY KEY (id_sale_payment),
    INDEX idx_sale_payment_method_date (payment_method, created_at),
    CONSTRAINT fk_sale_payment_sale
        FOREIGN KEY (id_sale) REFERENCES sale (id_sale)
        ON UPDATE CASCADE
        ON DELETE CASCADE,
    CONSTRAINT chk_sale_payment_amount CHECK (amount > 0),
    CONSTRAINT chk_sale_payment_installments CHECK (installments >= 1)
) ENGINE=InnoDB;

-- ==========================================================
-- 4. TRIGGERS DE ESTOQUE, TOTAIS E STATUS DE PAGAMENTO
-- ==========================================================

DELIMITER $$

-- 4.1 Calcula subtotal do item e valida estoque ao inserir (somente se for VENDA)
CREATE TRIGGER trg_sale_product_before_insert
BEFORE INSERT ON sale_product
FOR EACH ROW
BEGIN
    DECLARE v_estoque_atual DECIMAL(12,3);
    DECLARE v_tipo_venda VARCHAR(30);

    SELECT sale_type INTO v_tipo_venda
    FROM sale
    WHERE id_sale = NEW.id_sale;

    IF v_tipo_venda = 'VENDA' THEN
        SELECT stock_quantity INTO v_estoque_atual
        FROM product
        WHERE id_product = NEW.id_product;

        IF v_estoque_atual < NEW.quantity THEN
            SIGNAL SQLSTATE '45000'
            SET MESSAGE_TEXT = 'Estoque insuficiente para realizar a venda deste produto.';
        END IF;
    END IF;

    SET NEW.subtotal = (NEW.quantity * NEW.unit_price) - NEW.discount_product;
END$$

CREATE TRIGGER trg_sale_product_after_insert
AFTER INSERT ON sale_product
FOR EACH ROW
BEGIN
    DECLARE v_tipo_venda VARCHAR(30);

    SELECT sale_type INTO v_tipo_venda
    FROM sale
    WHERE id_sale = NEW.id_sale;

    IF v_tipo_venda = 'VENDA' THEN
        UPDATE product
        SET stock_quantity = stock_quantity - NEW.quantity
        WHERE id_product = NEW.id_product;
    END IF;

    UPDATE sale
    SET subtotal = (
            SELECT COALESCE(SUM(subtotal), 0.00)
            FROM sale_product
            WHERE id_sale = NEW.id_sale
        ),
        total = GREATEST(0.00, (
            SELECT COALESCE(SUM(subtotal), 0.00)
            FROM sale_product
            WHERE id_sale = NEW.id_sale
        ) - discount)
    WHERE id_sale = NEW.id_sale;
END$$

-- 4.2 Valida saldo e ajusta estoque/totais ao atualizar item
CREATE TRIGGER trg_sale_product_before_update
BEFORE UPDATE ON sale_product
FOR EACH ROW
BEGIN
    DECLARE v_estoque_atual DECIMAL(12,3);
    DECLARE v_tipo_venda VARCHAR(30);

    SELECT sale_type INTO v_tipo_venda
    FROM sale
    WHERE id_sale = NEW.id_sale;

    IF v_tipo_venda = 'VENDA' AND NEW.quantity > OLD.quantity THEN
        SELECT stock_quantity INTO v_estoque_atual
        FROM product
        WHERE id_product = NEW.id_product;

        IF v_estoque_atual < (NEW.quantity - OLD.quantity) THEN
            SIGNAL SQLSTATE '45000'
            SET MESSAGE_TEXT = 'Estoque insuficiente para aumentar a quantidade deste produto.';
        END IF;
    END IF;

    SET NEW.subtotal = (NEW.quantity * NEW.unit_price) - NEW.discount_product;
END$$

CREATE TRIGGER trg_sale_product_after_update
AFTER UPDATE ON sale_product
FOR EACH ROW
BEGIN
    DECLARE v_tipo_venda VARCHAR(30);

    SELECT sale_type INTO v_tipo_venda
    FROM sale
    WHERE id_sale = NEW.id_sale;

    IF v_tipo_venda = 'VENDA' THEN
        UPDATE product
        SET stock_quantity = stock_quantity + OLD.quantity - NEW.quantity
        WHERE id_product = NEW.id_product;
    END IF;

    UPDATE sale
    SET subtotal = (
            SELECT COALESCE(SUM(subtotal), 0.00)
            FROM sale_product
            WHERE id_sale = NEW.id_sale
        ),
        total = GREATEST(0.00, (
            SELECT COALESCE(SUM(subtotal), 0.00)
            FROM sale_product
            WHERE id_sale = NEW.id_sale
        ) - discount)
    WHERE id_sale = NEW.id_sale;
END$$

-- 4.3 Estorna estoque e recalcula totais ao remover item
CREATE TRIGGER trg_sale_product_after_delete
AFTER DELETE ON sale_product
FOR EACH ROW
BEGIN
    DECLARE v_tipo_venda VARCHAR(30);

    SELECT sale_type INTO v_tipo_venda
    FROM sale
    WHERE id_sale = OLD.id_sale;

    IF v_tipo_venda = 'VENDA' THEN
        UPDATE product
        SET stock_quantity = stock_quantity + OLD.quantity
        WHERE id_product = OLD.id_product;
    END IF;

    UPDATE sale
    SET subtotal = (
            SELECT COALESCE(SUM(subtotal), 0.00)
            FROM sale_product
            WHERE id_sale = OLD.id_sale
        ),
        total = GREATEST(0.00, (
            SELECT COALESCE(SUM(subtotal), 0.00)
            FROM sale_product
            WHERE id_sale = OLD.id_sale
        ) - discount)
    WHERE id_sale = OLD.id_sale;
END$$

-- 4.4 Atualiza o status da venda automaticamente ao inserir pagamento
CREATE TRIGGER trg_sale_payment_after_insert
AFTER INSERT ON sale_payment
FOR EACH ROW
BEGIN
    DECLARE v_total_venda DECIMAL(12,2);
    DECLARE v_total_pago DECIMAL(12,2);

    SELECT total INTO v_total_venda
    FROM sale
    WHERE id_sale = NEW.id_sale;

    SELECT COALESCE(SUM(amount), 0.00) INTO v_total_pago
    FROM sale_payment
    WHERE id_sale = NEW.id_sale;

    IF v_total_pago >= v_total_venda AND v_total_venda > 0 THEN
        UPDATE sale
        SET payment_status = 'PAGO'
        WHERE id_sale = NEW.id_sale;
    ELSEIF v_total_pago > 0 THEN
        UPDATE sale
        SET payment_status = 'PARCIAL'
        WHERE id_sale = NEW.id_sale;
    END IF;
END$$

-- 4.5 Recalcula o status da venda caso um pagamento seja estornado/excluído
CREATE TRIGGER trg_sale_payment_after_delete
AFTER DELETE ON sale_payment
FOR EACH ROW
BEGIN
    DECLARE v_total_venda DECIMAL(12,2);
    DECLARE v_total_pago DECIMAL(12,2);

    SELECT total INTO v_total_venda
    FROM sale
    WHERE id_sale = OLD.id_sale;

    SELECT COALESCE(SUM(amount), 0.00) INTO v_total_pago
    FROM sale_payment
    WHERE id_sale = OLD.id_sale;

    IF v_total_pago >= v_total_venda AND v_total_venda > 0 THEN
        UPDATE sale
        SET payment_status = 'PAGO'
        WHERE id_sale = OLD.id_sale;
    ELSEIF v_total_pago > 0 THEN
        UPDATE sale
        SET payment_status = 'PARCIAL'
        WHERE id_sale = OLD.id_sale;
    ELSE
        UPDATE sale
        SET payment_status = 'PENDENTE'
        WHERE id_sale = OLD.id_sale;
    END IF;
END$$

-- ==========================================================
-- 5. STORED PROCEDURE PARA REGISTRAR PAGAMENTO COM SEGURANÇA
-- ==========================================================

CREATE PROCEDURE sp_registrar_pagamento_venda(
    IN p_id_sale BIGINT,
    IN p_payment_method VARCHAR(50),
    IN p_amount DECIMAL(10,2),
    IN p_installments INT
)
BEGIN
    DECLARE v_existe_venda INT DEFAULT 0;

    SELECT COUNT(*) INTO v_existe_venda
    FROM sale
    WHERE id_sale = p_id_sale;

    IF v_existe_venda = 0 THEN
        SIGNAL SQLSTATE '45000'
        SET MESSAGE_TEXT = 'Venda não encontrada.';
    END IF;

    INSERT INTO sale_payment (id_sale, payment_method, amount, installments)
    VALUES (p_id_sale, p_payment_method, p_amount, COALESCE(p_installments, 1));
END$$

DELIMITER ;

-- ==========================================================
-- 6. VIEW RESUMO DE VENDAS (RELATÓRIO RÁPIDO)
-- ==========================================================

CREATE OR REPLACE VIEW vw_resumo_vendas AS
SELECT 
    sale.id_sale,
    sale.sale_date,
    customer.name AS nome_cliente,
    sale.seller_name AS vendedor,
    sale.sale_type AS tipo_venda,
    sale.subtotal,
    sale.discount AS desconto,
    sale.total,
    sale.payment_status AS status_pagamento,
    COALESCE((
        SELECT SUM(sale_payment.amount)
        FROM sale_payment
        WHERE sale_payment.id_sale = sale.id_sale
    ), 0.00) AS total_pago
FROM sale
INNER JOIN customer ON sale.id_client = customer.id_customer;


-- ==========================================================
-- 7. NOTIFICAÇÕES DE ESTOQUE BAIXO (MySQL)
-- ==========================================================

CREATE TABLE IF NOT EXISTS low_stock_notifications (
    id BIGINT NOT NULL AUTO_INCREMENT PRIMARY KEY,
    product_id INT NOT NULL,
    product_name VARCHAR(200) NOT NULL,
    stock_quantity DECIMAL(12,3) NOT NULL,
    min_stock DECIMAL(12,3) NOT NULL,
    created_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
    displayed_at TIMESTAMP NULL,
    INDEX idx_low_stock_product (product_id),
    INDEX idx_low_stock_displayed (displayed_at),
    CONSTRAINT fk_low_stock_product FOREIGN KEY (product_id) REFERENCES product (id_product) ON UPDATE CASCADE ON DELETE CASCADE
) ENGINE=InnoDB;

DROP TRIGGER IF EXISTS trg_product_stock_alert;
DROP TRIGGER IF EXISTS trg_product_stock_alert_update;

DELIMITER $$

CREATE TRIGGER trg_product_stock_alert
AFTER INSERT ON product
FOR EACH ROW
BEGIN
    IF NEW.is_active = TRUE AND NEW.stock_quantity <= NEW.min_stock THEN
        INSERT INTO low_stock_notifications (product_id, product_name, stock_quantity, min_stock)
        VALUES (NEW.id_product, NEW.name, NEW.stock_quantity, NEW.min_stock);
    END IF;
END$$

CREATE TRIGGER trg_product_stock_alert_update
AFTER UPDATE ON product
FOR EACH ROW
BEGIN
    IF NEW.is_active = TRUE
       AND NEW.stock_quantity <= NEW.min_stock
       AND (OLD.is_active = FALSE OR OLD.stock_quantity > OLD.min_stock OR OLD.min_stock <> NEW.min_stock OR OLD.name <> NEW.name) THEN
        INSERT INTO low_stock_notifications (product_id, product_name, stock_quantity, min_stock)
        VALUES (NEW.id_product, NEW.name, NEW.stock_quantity, NEW.min_stock);
    END IF;
END$$

DELIMITER ;

