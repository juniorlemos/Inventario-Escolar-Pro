# 📦 Inventário-360 — Sistema de Gestão de Patrimônio Escolar

## 📘 Sobre o Projeto
O **Inventário-360** é um sistema web desenvolvido para auxiliar escolas no **controle e gerenciamento de equipamentos, móveis, materiais e demais patrimônios**.  
Seu objetivo é oferecer uma solução **simples, eficiente e escalável**, garantindo **segurança, rastreabilidade e organização** na administração dos recursos escolares.  

Pensado para atender demandas reais da rotina escolar, o sistema reduz perdas, facilita auditorias e promove **transparência no processo de inventário**.

---

## 🚀 Funcionalidades
- 📚 **Gestão de Escolas**  
  Cadastro de instituições, setores, blocos e salas  

- 👤 **Gestão de Usuários (Microsoft Identity)**  
  Autenticação e autorização com roles  
  Login via JWT  
  Acesso privilegiado para administradores  
  Recuperação de senha por email (via Brevo)  

- 📦 **Cadastro de Itens**  
  Registro completo de equipamentos e materiais  
  Controle de quantidade, categoria, estado e localização  

- 🔄 **Movimentação de Itens**  
  Transferência entre salas  
  Histórico detalhado de movimentações  

- 📊 **Relatórios**  
  Geração de PDFs com **QuestPDF**  
  Indicadores e estatísticas úteis  

---

## 🏛️ Arquitetura
- Clean Architecture (Domain, Application, Infrastructure, Presentation)  
- Padrões **CQRS** com **MediatR**

---

## 🛠️ Tecnologias Utilizadas
**Backend**  
- .NET 9 / ASP.NET Core  
- Entity Framework Core (PostgreSQL / SQLServer)  
- Microsoft Identity + JWT  
- MediatR, FluentValidation, Serilog  

**Frontend**  
- Angular 19  

**Infraestrutura**  
- Docker & Docker Compose  
- FluentMigrator  
- Swagger / OpenAPI  

**Relatórios**  
- QuestPDF  

---

## 🧪 Testes Automatizados
- **Ferramentas**: xUnit, Shouldly, NSubstitute, Bogus  
- **Tipos de Testes**:  
  - Unitários (Domain, Application)  
  - Validações com FluentValidation  

🎯 Objetivo: Garantir estabilidade, evitar regressões e validar regras críticas antes do deploy.

---

## 🤖 CI/CD com GitHub Actions
Pipeline configurado para:  
- Build automático  
- Execução de testes  
- Criação de imagem Docker  
- Push automático para o Docker Hub  

📌 Cada commit na branch principal gera uma nova imagem publicada no Docker Hub.

---

## 🐳 Deploy no Docker Hub
A aplicação está totalmente dockerizada:  
- Backend (.NET)  
- Frontend (Angular)  
- Banco PostgreSQL (com volume persistente)  

📍 Demo online: [Inventário-360](https://inventario360-front.onrender.com)  
Login: `admin@escola.com`  
Senha: `Admin@123`

---

## 📥 Como Rodar Localmente com Docker

```bash
# 1️⃣ Clonar o repositório
git clone https://github.com/juniorlemos/inventario-escolar.git
cd inventario-escolar

# 2️⃣ Criar o arquivo .env
cp .env.example .env
# 👉 Preencha as variáveis de ambiente necessárias (DB, JWT_SECRET, PORT etc.) e no frontend como http://localhost:8080/    

# 3️⃣ Subir os containers
docker compose up -d

🌐 Acessar a aplicação
Front-end: http://localhost

API (Swagger): http://localhost:8080/swagger

🔑 Credenciais padrão
Login: admin@escola.com
Senha: Admin@123


## 📬 Contato e Sugestões
Se você tiver qualquer dúvida, encontrar algum problema ou quiser sugerir melhorias para o **Inventário-360**, fique à vontade para entrar em contato comigo.

Toda contribuição é bem-vinda! 🚀
