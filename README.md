# 🍽️ RecipeBook - Projeto Acadêmico

Este é um projeto acadêmico desenvolvido como parte de um curso voltado para aprimoramento técnico em desenvolvimento backend com foco em **boas práticas**, **arquitetura de software** e **entregas ágeis**.

O sistema consiste em uma API para gerenciamento de receitas culinárias com autenticação segura, integrações modernas e padrões de qualidade utilizados no mercado.

---

## 🚀 Tecnologias e Práticas Utilizadas

### 🧠 Arquitetura & Design
- 🔷 **Domain-Driven Design (DDD)**
- 🧱 **SOLID Principles**
- 🧼 **Clean Code**
- 🧪 **Testes de Unidade e Integração**
- 📦 **Injeção de Dependência (DI)** com `Microsoft.Extensions.DependencyInjection`
- 🧭 **Design modular e separação de responsabilidades**

### 🛠️ Ferramentas & DevOps
- ☁️ **Azure DevOps** para boards, repositórios e pipelines
- 🔄 **CI/CD com Pipelines**
- 🐳 **Docker** para containerização da aplicação
- 🔍 **SonarCloud & SonarQube** para análise estática de código
- 🧪 **FluentValidation** para validação de dados
- 📚 **Migrations do banco de dados** via Entity Framework

### ⚙️ Backend & Infraestrutura
- 🌐 **ASP.NET Core**
- 🗄️ **Entity Framework Core**
- 🔐 **Autenticação com Google Login**
- 🛡️ **JWT e Refresh Tokens** para autenticação segura
- 📬 **Mensageria (eventualmente com RabbitMQ ou outros brokers)**

### 📈 Metodologia
- 🧩 **SCRUM** com sprints e entregas contínuas
- 🧪 **Git & GitFlow** para versionamento e fluxo de desenvolvimento
- 🤖 **Integração com ChatGPT** como apoio à produtividade e revisão

---

## 📸 Funcionalidades

- Cadastro, edição e remoção de receitas
- Autenticação e login com conta Google
- Perfil de usuário e alteração de senha
- Tokens JWT + Refresh Token
- Integração com API externa (ex: WakeCommerce)
- Logs e tratamento global de exceções
- Testes automatizados e cobertura de código
- Regras de negócio validadas com FluentValidation
- Mensageria para processamento assíncrono (em desenvolvimento)

---

## 🏁 Como executar

```bash
# Clonar o repositório
git clone https://github.com/brunopsouz/clean-architecture-ddd.git

# Subir os containers com Docker
docker-compose up --build

# Acessar a API via:
http://localhost:5000/swagger
```
---

## 📄 Licença
Projeto acadêmico para fins educacionais. Sem uso comercial.
