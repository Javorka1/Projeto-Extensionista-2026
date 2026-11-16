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
	active BOOLEAN NOT NULL DEFAULT TRUE,
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
