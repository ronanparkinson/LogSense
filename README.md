# LogSense

> **AI-powered log management and observability platform built with ASP.NET Core, PostgreSQL, OpenSearch, Microsoft.Extensions.AI, locally hosted LLM inference with Ollama, Clean Architecture, and planned agentic AI capabilities.**

---

## Overview

LogSense is a personal AI/software engineering project focused on building a production-style log ingestion, search, and analysis platform.

The current system provides a layered ASP.NET Core backend that persists structured logs to PostgreSQL, indexes them into OpenSearch for full-text operational search, and passes retrieved log context to a locally hosted LLM for structured AI-assisted incident analysis. The AI layer uses Microsoft.Extensions.AI and its `IChatClient` abstraction with Ollama as the local inference provider. Future stages add semantic retrieval/RAG, AI-driven workflows, agent-facing APIs/tools, and a React/TypeScript frontend.

The project is intended to demonstrate practical backend engineering, search infrastructure, observability, AI integration, and agentic application design in one end-to-end system.

---

## Current Architecture

```text
                         HTTP Request
                              |
                              v
                       ASP.NET Core API
                              |
                              v
                         Controllers
                              |
                              v
                         Application
                           Services
                              |
                 +------------+-------------+
                 |                          |
                 v                          v
          Repository Layer          OpenSearch Service
                 |                          |
                 v                          v
        Entity Framework Core          OpenSearch
                 |
                 v
             PostgreSQL
```

Incoming logs are persisted in PostgreSQL and indexed into OpenSearch. Query endpoints support structured database filtering as well as full-text OpenSearch queries.

---

## Current Features

### Backend

- RESTful ASP.NET Core Web API
- Structured log ingestion
- PostgreSQL persistence
- Entity Framework Core
- Database migrations
- Domain models
- Data Transfer Objects (DTOs)
- Model validation
- Global exception handling middleware
- Async service and repository operations
- Log querying and filtering
- Date-range querying
- Serilog structured application logging

### Search & Log Management

- OpenSearch integration using the .NET client
- Automatic OpenSearch indexing during log ingestion
- Full-text log search
- Multi-field search across log message, exception, and source
- Newest-first search results
- Dockerized OpenSearch
- OpenSearch Dashboards
- Separate PostgreSQL and search responsibilities

### AI Log Analysis

- Local LLM inference using Ollama
- Microsoft.Extensions.AI `IChatClient` abstraction
- OllamaSharp integration
- Qwen3 1.7B as the fast local development model
- Qwen3 4B available for higher-quality analysis/evaluation
- OpenSearch-to-LLM analysis pipeline
- Structured AI analysis responses
- Incident summaries and likely root-cause suggestions
- Severity assessment and affected-service identification
- Recommended remediation actions
- Bounded log context sent to the model
- AI service failure handling and internal logging
- No hosted AI API key or per-request cloud inference dependency

### Architecture

- Clean Architecture
- Dependency Injection
- Service Layer
- Repository Pattern
- Infrastructure abstractions
- Separation of Concerns
- SOLID principles

### Development

- Layered solution structure
- Git feature-branch workflow
- Feature-based development
- Docker Compose for local search infrastructure

---

## Technology Stack

### Currently Implemented

**Backend**
- C#
- ASP.NET Core
- Entity Framework Core
- PostgreSQL
- Serilog
- REST APIs

**Search & Infrastructure**
- OpenSearch
- OpenSearch .NET Client
- OpenSearch Dashboards
- Docker
- Docker Compose

**AI**
- Microsoft.Extensions.AI
- `IChatClient`
- Ollama (local inference)
- OllamaSharp 5.4.30
- Qwen3 1.7B (development)
- Qwen3 4B (quality evaluation)
- Structured LLM outputs
- AI-assisted log and incident analysis

**Architecture**
- Clean Architecture
- Repository Pattern
- Dependency Injection
- Service Layer
- DTO-based API boundaries
- Global middleware

### Planned AI & Agentic Technologies

- Microsoft Agent Framework / Microsoft agentic tooling
- AI-driven operational workflows
- Agent-facing APIs and tool contracts
- Retrieval-Augmented Generation (RAG)
- Vector embeddings and semantic search
- Natural-language log investigation
- Structured AI outputs for machine/agent consumption
- Python for supporting AI/ML components

### Planned Frontend & Observability

- TypeScript
- React
- Prometheus
- Grafana
- Health checks and operational metrics

### Planned Delivery / Cloud

- GitHub Actions CI/CD
- Kubernetes
- Terraform
- Cloud deployment

---

## Local AI Runtime

LogSense currently performs AI inference locally rather than relying on a hosted, billable LLM API.

**Runtime**
- Ollama 0.32.13
- Ollama API endpoint: `http://localhost:11434`
- OllamaSharp 5.4.30
- Microsoft.Extensions.AI via `IChatClient`

**Models**
- `qwen3:1.7b` — primary development model, selected for faster CPU-only inference
- `qwen3:4b` — higher-capacity model retained for quality comparison and evaluation

The model is configurable through application configuration, allowing the underlying model/provider to change without coupling `LogAnalysisService` directly to Ollama or Qwen.

```text
OpenSearch
    |
    v
Relevant bounded log context
    |
    v
LogAnalysisService
    |
    v
Microsoft.Extensions.AI / IChatClient
    |
    v
OllamaSharp
    |
    v
Ollama (localhost)
    |
    v
Qwen3 local model
    |
    v
Structured LogAnalysisResponse
```

This local setup requires no hosted AI API key and performs model inference on the development machine.

---

## Solution Structure

```text
LogSense
|
|-- LogSense.Api
|   |-- Controllers
|   |-- Middleware
|   `-- Configuration
|
|-- LogSense.Application
|   |-- DTOs
|   |-- Interfaces
|   |-- Services
|   `-- Business Logic
|
|-- LogSense.Domain
|   |-- Entities
|   `-- Models
|
|-- LogSense.Infrastructure
|   |-- Configuration
|   |-- Migrations
|   |-- Persistence
|   |-- Repositories
|   `-- Services
|
`-- docker-compose.yml
```

---

# Roadmap

## ✅ Phase 1 – Backend Foundation

- [x] ASP.NET Core Web API
- [x] PostgreSQL
- [x] Entity Framework Core
- [x] Database migrations
- [x] DTO validation
- [x] Global exception middleware
- [x] Dependency Injection
- [x] Repository Pattern
- [x] Service Layer
- [x] Clean Architecture
- [x] Async data access

---

## ✅ Phase 2 – Core Log Management

- [x] Structured log ingestion
- [x] Log persistence
- [x] Log querying
- [x] Level filtering
- [x] Date-range filtering
- [x] Structured logging with Serilog
- [x] OpenSearch integration
- [x] Automatic OpenSearch indexing
- [x] Full-text search
- [x] Multi-field search
- [x] Newest-first OpenSearch results

### Further backend refinements

- [ ] Pagination
- [ ] Health checks
- [ ] Unit testing
- [ ] Integration testing
- [ ] Additional query/filter options

---

## ✅ Phase 3 – AI Log Analysis

- [x] AI-generated log summaries
- [x] Incident summarization
- [x] Likely root-cause suggestions
- [x] Severity assessment
- [x] Affected-service identification
- [x] Recommended remediation actions
- [x] Structured AI analysis responses

Example:

> "Analyse these PaymentService errors and identify the most likely cause."

---

## 🤖 Phase 4 – AI-Driven Workflows & Agentic AI

- [ ] AI-driven log investigation workflows
- [ ] Microsoft Agent Framework integration
- [ ] Agent orchestration for operational investigations
- [ ] Agent-accessible search tools
- [ ] Agent-accessible log analysis tools
- [ ] APIs designed for AI-agent consumption
- [ ] Structured tool inputs and outputs
- [ ] Agent-safe validation and error responses
- [ ] Multi-step investigation workflows

Example workflow:

```text
Incident detected
      |
      v
Search related logs
      |
      v
Analyse errors
      |
      v
Identify likely root cause
      |
      v
Generate recommended actions
```

---

## 🧠 Phase 5 – Semantic Search & RAG

- [ ] Vector embeddings
- [ ] Semantic log search
- [ ] Retrieval-Augmented Generation (RAG)
- [ ] Natural-language operational queries
- [ ] Context retrieval from logs and supporting documentation

Example:

> "Why did the payment service fail yesterday?"

---

## 📈 Phase 6 – Machine Learning

- [ ] Log anomaly detection
- [ ] Error clustering
- [ ] Predictive failure detection
- [ ] Model evaluation
- [ ] Optional MLflow experiment/model tracking

---

## 🖥️ Phase 7 – React / TypeScript Frontend

- [ ] React application
- [ ] TypeScript
- [ ] Log search interface
- [ ] Filterable log views
- [ ] AI analysis interface
- [ ] Incident investigation views
- [ ] Operational dashboard

---

## ☁️ Phase 8 – Observability, CI/CD & Deployment

- [x] Docker
- [x] Docker Compose
- [ ] GitHub Actions CI/CD
- [ ] Prometheus metrics
- [ ] Grafana dashboards
- [ ] Alerting
- [ ] Kubernetes
- [ ] Terraform
- [ ] Cloud deployment

---

# AI-Driven Workflow Direction

A major goal of LogSense is to move beyond simply storing and searching logs.

The implemented AI analysis layer retrieves relevant logs through OpenSearch and passes bounded log context to a locally hosted Qwen model through Microsoft.Extensions.AI and Ollama. It produces structured incident analysis and recommended actions. Future agentic phases will expose these capabilities as tools in higher-level investigation workflows.

This allows the project to demonstrate the distinction between:

- conventional REST APIs used by applications and users,
- AI-assisted workflows that combine retrieval and reasoning, and
- APIs/tools intentionally designed for autonomous AI-agent consumption.

---

# Agent-Facing API Direction

LogSense will expose selected operational capabilities as deterministic, well-described tools suitable for AI agents.

Planned agent-facing capabilities include:

- Search logs by natural-language or structured criteria
- Retrieve recent errors for a service
- Retrieve logs within a time range
- Analyse a set of related log entries
- Summarize an incident
- Suggest likely root causes
- Produce structured recommended actions

Agent-facing endpoints/tools will favour:

- Explicit input schemas
- Structured JSON responses
- Predictable error handling
- Bounded result sets
- Clear tool descriptions
- Correlation and traceability
- Safe, deterministic operations where possible

---

# Engineering Principles

This project follows modern software engineering practices including:

- Clean Architecture
- SOLID principles
- Dependency Injection
- Repository Pattern
- Service Layer
- Separation of Concerns
- REST API design
- Async programming
- Structured logging
- Search-engine integration
- Testable architecture
- Feature-branch development
- Infrastructure abstraction

---

# Skills Demonstrated

### Backend Engineering

- ASP.NET Core
- C#
- Entity Framework Core
- PostgreSQL
- REST API development
- Async programming
- Dependency Injection
- Clean Architecture
- Repository Pattern
- Middleware and exception handling
- DTO validation

### Search & Observability

- OpenSearch
- Full-text search
- Search indexing
- Multi-field querying
- Dockerized infrastructure
- Serilog structured logging
- Log ingestion and investigation workflows

### AI Engineering

- Local LLM integration with Ollama
- Microsoft.Extensions.AI / `IChatClient`
- OllamaSharp
- Qwen3 1.7B and Qwen3 4B local models
- Structured AI outputs
- AI-assisted incident summarization
- AI-assisted root-cause analysis
- Severity assessment and remediation recommendations
- Provider-abstracted AI architecture
- Bounded model context and AI failure handling

**Planned / In Development**
- Microsoft Agent Framework
- AI-driven workflows
- Agentic AI
- Agent-facing API/tool development
- RAG
- Semantic search
- Embeddings
- Natural-language querying

### Frontend / Platform – Planned

- React
- TypeScript
- Prometheus
- Grafana
- GitHub Actions
- Kubernetes
- Terraform

---

# Long-Term Vision

LogSense is intended to evolve into an AI-assisted observability platform that helps engineers investigate production systems more effectively.

Rather than requiring an engineer to manually search large volumes of logs, the completed system will combine structured log storage, full-text and semantic retrieval, AI analysis, and agentic workflows to help answer operational questions and guide incident investigation.

The final platform is intended to demonstrate an end-to-end engineering workflow:

```text
Log ingestion
      |
      v
PostgreSQL + OpenSearch
      |
      v
Full-text / semantic retrieval
      |
      v
AI log analysis
      |
      v
AI-driven workflows
      |
      v
Agent-facing tools/APIs
      |
      v
React / TypeScript investigation UI
```

---

## Project Status

🚧 **Actively under development**

### Completed

- Backend architecture and persistence
- Log ingestion
- Structured querying and filtering
- Serilog integration
- Dockerized OpenSearch environment
- OpenSearch indexing
- Full-text multi-field search
- Local Ollama LLM integration
- Microsoft.Extensions.AI / `IChatClient` integration
- OpenSearch-to-LLM log analysis pipeline
- Structured AI incident analysis
- AI error handling and bounded model context

### Currently Moving Into

**Semantic retrieval/RAG and AI-driven operational workflows.**

Future phases will extend this foundation with Microsoft agentic tooling, agent-facing APIs, semantic retrieval/RAG, React/TypeScript, observability, testing, and deployment automation.
