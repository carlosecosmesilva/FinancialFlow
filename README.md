# 💰 FinancialFlow - Sistema de Gestão Financeira Pessoal

<div align="center">

![.NET](https://img.shields.io/badge/.NET-8-512BD4?logo=dotnet)
![Angular](https://img.shields.io/badge/Angular-16-DD0031?logo=angular)
![PostgreSQL](https://img.shields.io/badge/PostgreSQL-15-4169E1?logo=postgresql)
![Redis](https://img.shields.io/badge/Redis-7-DC382D?logo=redis)
![Docker](https://img.shields.io/badge/Docker-24.0-2496ED?logo=docker)

**Sistema completo de controle financeiro pessoal desenvolvido com Clean Architecture, DDD e SOLID principles**

[🚀 Começando](#-começando) • [🏗️ Arquitetura](#️-arquitetura) • [📊 Funcionalidades](#-funcionalidades) • [🛠️ Tecnologias](#️-tecnologias)

</div>

## 📋 Sobre o Projeto

O **FinancialFlow** é um sistema de gestão financeira pessoal que ajuda usuários a controlarem receitas, despesas, dívidas e investimentos. Desenvolvido como case study para demonstrar expertise em arquitetura de software avançada, patterns e boas práticas de desenvolvimento.

### 🎯 Objetivos Técnicos

- Demonstrar implementação de **Clean Architecture** e **Domain-Driven Design (DDD)**
- Aplicar **SOLID principles** e **design patterns** avançados
- Criar uma codebase **testável e maintainable**
- Implementar **CQRS** e **Domain Events**
- Containerização completa com **Docker**

---

## 🏗️ Arquitetura

### 📐 Clean Architecture

```
FinancialFlow/
├── 🎯 Domain/          → Entities, Value Objects, Domain Services
├── ⚙️ Application/     → Use Cases, DTOs, CQRS, Domain Events  
├── 🗃️ Infrastructure/  → Data Access, External Services, Cache
└── 🌐 API/            → Controllers, Middlewares, Configurations
```

### 🔧 Principais Patterns

- **CQRS** - Separação de comandos e queries
- **Repository Pattern** - Abstraction de data access
- **Unit of Work** - Gerenciamento de transações
- **Domain Events** - Business events desacoplados
- **Strategy Pattern** - Cálculos e exportações
- **Specification Pattern** - Queries complexas

---

## 🛠️ Stack Tecnológica

### Backend
- **.NET 8** - Framework principal
- **Entity Framework Core** - ORM
- **PostgreSQL** - Banco de dados principal
- **Redis** - Cache e session storage
- **MediatR** - CQRS implementation
- **FluentValidation** - Validation rules
- **AutoMapper** - Object mapping
- **xUnit** + **Moq** - Testing

### Frontend
- **Angular 16** - Framework frontend
- **TypeScript** - Language
- **RxJS** - Reactive programming
- **Chart.js** - Data visualization
- **Angular Material** - UI components

### Infra & DevOps
- **Docker** + **Docker Compose** - Containerization
- **GitHub Actions** - CI/CD
- **Nginx** - Reverse proxy

---

## 📊 Funcionalidades

### 💰 Gestão Financeira
- ✅ Controle de receitas e despesas
- ✅ Categorização automática de transações
- ✅ Importação de planilhas Excel
- ✅ Orçamento por categorias
- ✅ Projeções financeiras

### 🏦 Controle de Dívidas
- ✅ Cadastro de dívidas e parcelamentos
- ✅ Cálculo de juros e amortização
- ✅ Alertas de vencimento
- ✅ Priorização de pagamentos

### 📈 Gestão de Investimentos
- ✅ Múltiplos tipos (Ações, Tesouro, Renda Fixa)
- ✅ Acompanhamento de performance
- ✅ Projeção de retorno
- ✅ Diversificação de carteira

### 📊 Analytics & Relatórios
- ✅ Dashboard com métricas chave
- ✅ Gráficos interativos
- ✅ Relatórios personalizados
- ✅ Export PDF/Excel

### 🔔 Alertas Inteligentes
- ✅ Orçamento excedido
- ✅ Vencimento de dívidas
- ✅ Oportunidades de investimento
- ✅ Metas financeiras

---

## 🚀 Começando

### Pré-requisitos

- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- [Node.js 18+](https://nodejs.org/)
- [Docker](https://www.docker.com/get-started)
- [Docker Compose](https://docs.docker.com/compose/install/)

### 📥 Instalação Rápida

```bash
# Clone o repositório
git clone https://github.com/seu-usuario/financialflow.git
cd financialflow

# Suba a infraestrutura
docker-compose up -d postgres redis

# Execute a aplicação
docker-compose up --build
```

A aplicação estará disponível em:
- **Frontend**: http://localhost:4200
- **Backend API**: http://localhost:5000
- **PostgreSQL**: localhost:5432
- **Redis**: localhost:6379

### 🛠️ Desenvolvimento

```bash
# Backend (terminal 1)
cd src/FinancialFlow.API
dotnet run

# Frontend (terminal 2)  
cd src/FinancialFlow.Angular
npm install
ng serve

# Infraestrutura (terminal 3)
docker-compose up -d postgres redis
```

### 🧪 Testes

```bash
# Unit Tests
dotnet test tests/FinancialFlow.UnitTests/

# Integration Tests  
dotnet test tests/FinancialFlow.IntegrationTests/

# Frontend Tests
cd src/FinancialFlow.Angular
npm test
```

---

## 📁 Estrutura do Projeto

```
FinancialFlow/
├── 📁 src/
│   ├── FinancialFlow.API/           # Web API
│   ├── FinancialFlow.Application/   # Use Cases & CQRS
│   ├── FinancialFlow.Domain/        # Domain Model
│   ├── FinancialFlow.Infrastructure/# Data & External Services
│   └── FinancialFlow.Angular/       # Frontend App
├── 📁 tests/
│   ├── FinancialFlow.UnitTests/     # Unit Tests
│   └── FinancialFlow.IntegrationTests/# Integration Tests
├── 📁 docker/                       # Docker Configuration
├── 📁 scripts/                      # Build & Deployment
└── 📁 docs/                         # Documentation
```

---

## 🔧 Configuração

### Variáveis de Ambiente

Crie um arquivo `.env` na raiz do projeto:

```env
# Database
POSTGRES_DB=FinancialFlow
POSTGRES_USER=postgres
POSTGRES_PASSWORD=YourStrongPassword123

# Redis
REDIS_CONNECTION=localhost:6379

# Application
ASPNETCORE_ENVIRONMENT=Development
ALLOWED_ORIGINS=http://localhost:4200
```

### Configuração do Banco

```bash
# Criar migration
cd src/FinancialFlow.Infrastructure
dotnet ef migrations add InitialCreate --startup-project ../FinancialFlow.API

# Aplicar migration
dotnet ef database update --startup-project ../FinancialFlow.API
```

---

## 🏆 Diferenciais Técnicos

### 🎯 Clean Architecture
- Separação clara de responsabilidades
- Dependências invertidas
- Domínio rico e independente

### 📚 DDD Implementation
- Aggregates e Value Objects
- Domain Services e Domain Events
- Ubiquitous Language

### ⚡ Performance
- Cache Redis para queries frequentes
- CQRS para otimização de leitura/escrita
- Paginação e lazy loading

### 🔒 Segurança
- JWT Authentication
- Validações em múltiplas camadas
- Autorização baseada em recursos

### 🧪 Testabilidade
- Arquitetura test-friendly
- Mocks e fakes facilitados
- High test coverage

---

## 🤝 Contribuindo

1. Fork o projeto
2. Crie uma branch para sua feature (`git checkout -b feature/AmazingFeature`)
3. Commit suas mudanças (`git commit -m 'Add some AmazingFeature'`)
4. Push para a branch (`git push origin feature/AmazingFeature`)
5. Abra um Pull Request

### 📋 Guidelines

- Siga os princípios de Clean Architecture
- Mantenha a cobertura de testes
- Use Conventional Commits
- Documente novas funcionalidades

---

## 📄 Licença

Este projeto está sob a licença MIT. Veja o arquivo [LICENSE](LICENSE) para detalhes.

---

## 👨‍💻 Autor

**Carlos Silva** - [GitHub](https://github.com/carlosecosmesilva/legacy-bridge-vb6-csharp) - [LinkedIn](https://www.linkedin.com/in/carlosecdasilva/)

---

## 🙏 Agradecimentos

- Baseado nas planilhas de controle financeiro do livro referenciado
- Inspirado nos princípios de Clean Architecture do Uncle Bob
- Community .NET e Angular

---

<div align="center">

**⭐️ Considera dar uma estrela no repositório se este projeto te ajudou!**

[Report Bug](https://github.com/seu-usuario/financialflow/issues) • [Request Feature](https://github.com/seu-usuario/financialflow/issues)

</div>