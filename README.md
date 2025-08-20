# 🚀 Gerador de Dados Fake para Grafana

Este projeto gera dados fake de IoT, Logs e Métricas de Sistema em tempo real e os expõe para o Prometheus, que pode ser visualizado no Grafana.

## 📋 Pré-requisitos

- .NET 8 SDK
- Docker e Docker Compose
- Git (opcional)

## 🛠️ Instalação e Configuração

### 1. Preparar o projeto .NET

```bash
# Criar diretório do projeto
mkdir FakeDataGenerator
cd FakeDataGenerator

# Criar o arquivo .csproj (copie o conteúdo do arquivo de projeto fornecido)
# Criar o Program.cs (copie o código C# fornecido)

# Restaurar dependências
dotnet restore

# Compilar o projeto
dotnet build
```

### 2. Preparar o ambiente Docker

Crie a seguinte estrutura de diretórios:

```
FakeDataGenerator/
├── Program.cs
├── FakeDataGenerator.csproj
├── docker-compose.yml
├── prometheus.yml
├── grafana/
│   ├── provisioning/
│   │   ├── datasources/
│   │   │   └── datasource.yml
│   │   └── dashboards/
│   │       └── dashboard.yml
│   └── dashboards/
│       └── fake-data-dashboard.json
```

### 3. Configurar datasource do Grafana

Crie o arquivo `grafana/provisioning/datasources/datasource.yml`:

```yaml
apiVersion: 1

datasources:
  - name: Prometheus
    type: prometheus
    access: proxy
    url: http://prometheus:9090
    isDefault: true
    editable: true
```

### 4. Configurar dashboard do Grafana

Crie o arquivo `grafana/provisioning/dashboards/dashboard.yml`:

```yaml
apiVersion: 1

providers:
  - name: 'default'
    orgId: 1
    folder: ''
    type: file
    disableDeletion: false
    updateIntervalSeconds: 10
    allowUiUpdates: true
    options:
      path: /var/lib/grafana/dashboards
```

E copie o conteúdo do dashboard JSON fornecido para `grafana/dashboards/fake-data-dashboard.json`.

## ▶️ Executando o Sistema

### 1. Subir Prometheus e Grafana

```bash
# Executar no diretório raiz do projeto
docker-compose up -d
```

Aguarde alguns segundos para os containers iniciarem.

### 2. Executar o gerador de dados

```bash
# Em outro terminal, no diretório do projeto
dotnet run
```

Você verá logs como:

```
🚀 Gerador de dados fake iniciado!
📊 Métricas Prometheus disponíveis em: http://localhost:9090/metrics  
📝 Logs sendo gerados no console
🌡️  Dados IoT sendo simulados

Pressione CTRL+C para parar...

🌡️  Temp: 24.5°C - Device: sensor-001 - Local: Sala A
💧 Umidade: 67.2% - Device: sensor-002 - Local: Almoxarifado  
📊 Pressão: 1013.4hPa - Device: gateway-01 - Local: Telhado
⚡ Energia: 156.8W - Device: sensor-003 - Local: Sala B
ℹ️ [INFO] WebAPI: Usuário user_1234 fez login com sucesso
💻 Sistema - CPU: 45.2% | Mem: 62.1% | Disco: 78.3% | Conn: 89
```

## 🌐 Acessando as Interfaces

### Métricas do Prometheus (Raw)
- URL: `http://localhost:9090/metrics`
- Aqui você pode ver as métricas brutas que a aplicação está gerando

### Prometheus UI
- URL: `http://localhost:9091`
- Interface para consultar métricas e criar queries PromQL

### Grafana Dashboard
- URL: `http://localhost:3000`
- Usuário: `admin`
- Senha: `admin123`

## 📊 Métricas Disponíveis

### IoT
- `iot_temperature_celsius` - Temperatura dos sensores (°C)
- `iot_humidity_percent` - Umidade relativa (%)  
- `iot_pressure_hpa` - Pressão atmosférica (hPa)
- `iot_power_watts` - Consumo de energia (Watts)

### Sistema
- `system_cpu_usage_percent` - Uso de CPU (%)
- `system_memory_usage_percent` - Uso de memória (%)
- `system_disk_usage_percent` - Uso de disco (%)
- `system_network_in_bytes_total` - Bytes recebidos (counter)
- `system_network_out_bytes_total` - Bytes enviados (counter)
- `system_active_connections` - Conexões ativas
- `http_request_duration_seconds` - Tempo de resposta HTTP (histogram)

### Logs
- `log_entries_total` - Total de entradas de log por nível e fonte

## 🎯 Exemplos de Queries PromQL

```promql
# Taxa de CPU média nos últimos 5 minutos
avg(system_cpu_usage_percent)

# Temperatura máxima por localização
max(iot_temperature_celsius) by (location)

# Taxa de logs de erro por minuto
rate(log_entries_total{level="ERROR"}[1m])

# Percentil 95 do tempo de resposta
histogram_quantile(0.95, rate(http_request_duration_seconds_bucket[5m]))

# Tráfego de rede total
rate(system_network_in_bytes_total[1m]) + rate(system_network_out_bytes_total[1m])
```

## 🔧 Personalização

### Modificar dados IoT
Edite a classe `IoTDataGenerator` para alterar:
- Tipos de sensores
- Ranges de valores
- Localizações
- Nomes de dispositivos

### Adicionar novos tipos de logs
Edite a classe `LogDataGenerator` para incluir:
- Novos níveis de log
- Diferentes fontes/serviços
- Novos templates de mensagens

### Ajustar métricas de sistema
Modifique a classe `MetricsDataGenerator` para:
- Alterar ranges de valores
- Adicionar novas métricas
- Simular comportamentos específicos

### Customizar dashboard
- Importe o dashboard no Grafana
- Edite os painéis conforme necessário
- Adicione alertas e notificações
- Exporte e salve as alterações

## 🐛 Troubleshooting

### Aplicação não consegue conectar com Prometheus
- Verifique se o Prometheus está rodando: `docker ps`
- Confirme se a porta 9091 está livre
- Verifique os logs: `docker-compose logs prometheus`

### Grafana não mostra dados
- Verifique se o datasource está configurado corretamente
- Confirme se a aplicação .NET está rodando
- Teste queries no Prometheus primeiro: `http://localhost:9091`

### Performance
- Para ambientes de produção, ajuste os intervalos de scraping
- Configure retenção adequada de dados no Prometheus
- Monitore o uso de recursos dos containers

## 📝 Estrutura de Arquivos Final

```
FakeDataGenerator/
├── Program.cs                              # Código principal
├── FakeDataGenerator.csproj                # Arquivo do projeto
├── docker-compose.yml                      # Docker compose
├── prometheus.yml                          # Config do Prometheus
├── README.md                              # Este arquivo
├── grafana/
│   ├── provisioning/
│   │   ├── datasources/
│   │   │   └── datasource.yml            # Config do datasource
│   │   └── dashboards/
│   │       └── dashboard.yml             # Provisionamento de dashboards
│   └── dashboards/
│       └── fake-data-dashboard.json      # Dashboard principal
```

## 🚀 Próximos Passos

1. Execute tudo conforme as instruções
2. Acesse o Grafana e explore os dados
3. Customize conforme suas necessidades
4. Adicione alertas e notificações
5. Integre com seus sistemas reais

**Dica**: Deixe a aplicação rodando por alguns minutos para ver os gráficos de série temporal se popularem no Grafana! 📈