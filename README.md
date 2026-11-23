Projeto Interdisciplinar IV – Sistemas de Informação – UEMG, Unidade Frutal.
Aplicação desktop em C# (Windows Forms) com banco de dados MySQL, para registro de vendas e controle de estoque.

## Pré-requisitos
- Visual Studio com a carga de trabalho "Desenvolvimento para desktop com .NET"
- .NET Framework 4.8
- MySQL 8.0.16 ou superior e MySQL Workbench
- Git

## Como executar
### 1. Obter o código

```
(https://github.com/Javorka1/Projeto-Extensionista-2026)
```

### 2. Criar o banco de dados

1. Abra o MySQL Workbench e conecte ao servidor local.
2. Abra o arquivo `init.sql`, na pasta `SQL` do projeto.
3. Execute o script inteiro. Ele cria o banco `Sistema` com as tabelas, índices, triggers, procedures, função, view e o cliente padrão "Consumidor Final".

Para recriar o banco do zero, execute antes `DROP DATABASE IF EXISTS Sistema;` e rode o script novamente.

O script não cria usuários. Crie o primeiro usuário administrador:

```sql
USE Sistema;
CALL sp_create_user('admin', 'ESCOLHA_UMA_SENHA', 'admin');
```

Perfis disponíveis: admin, seller, stocker, monetary e common.

### 3. Configurar a conexão com o banco

1. Na pasta do projeto, copie o arquivo `.env.example` para `.env`.
2. Preencha servidor, banco (`Sistema`), usuário e senha do seu MySQL.

O arquivo `.env` contém a senha do banco e não deve ser enviado ao repositório.

### 4. Compilar

1. Abra o arquivo `.sln` no Visual Studio.
2. Aguarde a restauração dos pacotes do NuGet.
3. Acesse Compilar > Compilar Solução (`Ctrl+Shift+B`).
4. Na janela Saída, confirme que o resultado indica 0 com falha.

### 5. Executar

1. Pressione `F5` no Visual Studio.
2. Entre com o usuário criado no passo 2.

## Problemas comuns

- **Não conecta ao banco:** verifique se o MySQL está em execução e se os dados do `.env` estão corretos.
- **Erro ao entrar no sistema:** confirme que o usuário foi criado com o `CALL sp_create_user`.
- **Erro por falta de pacote:** no Visual Studio, restaure os pacotes em Ferramentas > Gerenciador de Pacotes do NuGet.
- **Erro nas restrições CHECK ao rodar o script:** atualize o MySQL para a versão 8.0.16 ou superior.

## Equipe

Davi Aidar, Gabriel Javorka Póvoa, Henrique Lopes Santos, Pablo Rodrigues Cardoso, Rebeca Cristina Moreira dos Santos e Roger Henrique Borges Viana.

Empresa parceira: 61.190.744 Tiago Rodrigues Cardoso – Frutal/MG.
