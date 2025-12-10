📦 Inventário-360 — Sistema de Gestão de Patrimônio para Escolas


📘 Sobre o Projeto

O Inventário-360 é um sistema desenvolvido para auxiliar escolas no controle e gerenciamento de seus equipamentos, móveis, materiais e demais patrimônios.
O objetivo é oferecer uma ferramenta simples, eficiente e escalável para que gestores possam administrar seus recursos com segurança, rastreabilidade e organização.

Este projeto foi pensado para atender demandas reais da rotina escolar, reduzindo perdas, facilitando auditorias e trazendo mais transparência para o processo de inventário.
🚀 Funcionalidades
📚 Gestão de Escolas

Cadastro e gerenciamento de escolas

Organização por setores, blocos e salas

👤 Gestão de Usuários (Microsoft Identity)

Autenticação e autorização com roles

Acesso privilegiado para administradores

Login com JWT

📦 Cadastro de Itens

Registro completo de equipamentos e materiais

Quantidade, categoria, estado e localização

🔄 Movimentação de Itens

Transferência entre salas

Histórico completo de movimentações

📊 Relatórios

Geração de relatórios PDF com QuestPDF

Indicadores e estatísticas úteis

🏛️ Arquitetura

Clean Architecture (Domain, Application, Infrastructure, Presentation)

Padrões CQRS com MediatR

🛠️ Tecnologias Utilizadas
Backend

.NET 9 / ASP.NET Core

Angular 19

Entity Framework Core (PostgreSQL)
SQLServer

Microsoft Identity + JWT

MediatR

FluentValidation

Serilog

Infraestrutura

Docker & Docker Compose

FluentMigrator

Documentação

Swagger / OpenAPI

Relatórios

QuestPDF

🧪 Testes Automatizados

O projeto possui uma suíte robusta de testes que garante previsibilidade e segurança nas regras de negócio.

✔️ Ferramentas de Teste

xUnit

Shouldly

NSubstitute

Bogus


✔️ Tipos de Testes

Testes de Unidades (Domain, Application)

Testes de validação com FluentValidation


🎯 Objetivo

Garantir estabilidade

Evitar regressões

Validar regras críticas antes do deploy


🤖 CI/CD com GitHub Actions

Este projeto possui integração contínua com GitHub Actions, incluindo:

Build automático da solução

Execução de testes

Criação da imagem Docker

Push automático da imagem para o Docker Hub

📌 Toda vez que um novo commit é enviado para a branch principal, a imagem atualizada é publicada automaticamente no Docker Hub.


🐳 Deploy no Docker Hub

A aplicação já está totalmente dockerizada:

Backend .NET → enviado automaticamente para o Docker Hub

Frontend Angular → enviado automaticamente para o Docker Hub

Banco PostgreSQL → container interno com volume persistente

.env.example → facilita para outros usuários reproduzirem a configuração

Você pode baixar, configurar e rodar o projeto apenas com Docker.

Para somente rodar o sistema online entre com o seguinte endereço
https://inventario360-front.onrender.com/
Login admin@escola.com
Senha Admin@123 
 é possivel analisar as funcionalidades do sistema em ação



📥 Como Rodar o Sistema em Qualquer PC Usando Docker
1️⃣ Baixar o repositório
git clone https://github.com/SEU_USUARIO/inventario-escolar.git
cd inventario-escolar

2️⃣ Criar o arquivo .env

Existe um .env.example com todas as variáveis necessárias.

Basta copiar:

cp .env.example .env

3️⃣ Rodar tudo com Docker Compose
docker compose up -d


Isso irá subir automaticamente:

Backend (.NET)

Frontend (Angular)

PostgreSQL

4️⃣ Acessar a aplicação

Abra no navegador:

http://localhost:4200


O frontend Angular já se conecta à API automaticamente.
Basta logar e começar a utilizar.

🔐 Usuário e Senha Padrão

Ao iniciar o sistema pela primeira vez, o processo de Data Seeding cria automaticamente um usuário Administrador para que você consiga acessar o painel imediatamente.

Credenciais padrão              Campo	Valor
Email / Login	                  admin@escola.com
Senha	                          Admin@123

Estas credenciais são criadas automaticamente no método DataSeeder, dentro do backend do sistema.

Caso deseje alterar o email ou a senha padrão, basta editar este trecho do código:

var adminUser = new ApplicationUser
{
    UserName = "admin@escola.com",
    Email = "admin@escola.com",
    EmailConfirmed = true,
    SchoolId = escola.Id,
    School = escola
};

var result = await userManager.CreateAsync(adminUser, "Admin@123");

📝 Observação Importante

Se o sistema já foi executado antes, o usuário Administrador já estará salvo no banco de dados.
Alterar o código acima não atualiza automaticamente o usuário no banco.

Se quiser usar novas credenciais do seed, você precisa:

Excluir o usuário manualmente no banco
ou

Apagar o banco de dados para permitir que o seed rode novamente.





