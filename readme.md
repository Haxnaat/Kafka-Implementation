# Kafka Minimal API Demo

This is a **.NET 7 Minimal API** project demonstrating a simple **Kafka Producer and Consumer** workflow with a browser UI.  

Users can enter messages in the UI, publish them to Kafka, and see messages streamed live from the consumer.

---

## Features

- Minimal API backend using **.NET 7**
- Kafka **Producer** (`/publish` endpoint)
- Kafka **Consumer** streaming via **Server-Sent Events** (`/stream` endpoint)
- Browser UI to:
  - Publish messages
  - Display live consumer messages
- Fully async using `Confluent.Kafka`  

---

## Requirements

- [.NET 7 SDK](https://dotnet.microsoft.com/en-us/download/dotnet/7.0)  
- [Docker](https://www.docker.com/) (for Kafka & Zookeeper)  
- Modern browser for UI  

---

## Setup

### 1. Clone the repository

```bash
git clone https://github.com/<your-username>/kafka-minimal-api-demo.git
cd kafka-minimal-api-demo
```

### 2. Start Kafka & Zookeeper using Docker

```bash
docker compose up -d
```

Check that Kafka is running:

```bash
docker ps
```

---

### 3. Run the API

```bash
dotnet run
```

You should see:

```
Now listening on: http://localhost:5279
```

---

### 4. Open the UI

Open a browser:

```
http://localhost:5279/
```

- Type a message → click **Publish**  
- Messages from Kafka will appear in the live **Consumer Messages** list  

---

## Project Structure

```
Kafka-Implementation/
 ├─ Program.cs           # Minimal API routes & setup
 ├─ Kafka/
 │   ├─ KafkaProducer.cs # Producer logic
 │   └─ KafkaConsumer.cs # Consumer logic
 ├─ wwwroot/
 │   └─ index.html       # Browser UI
 └─ Kafka-Implementation.csproj
```

---

## Endpoints

| Method | URL        | Description               |
|--------|------------|---------------------------|
| POST   | `/publish` | Publish message to Kafka  |
| GET    | `/stream`  | Server-Sent Events consumer stream |

---

## Debugging

- Open in **VS Code**
- Set breakpoints in `Program.cs`, `KafkaProducer.cs`, or `KafkaConsumer.cs`
- Press **F5** to launch debugger
- Use browser UI to trigger endpoints and watch variables  

---

## Dependencies

- `Confluent.Kafka`  
- `System.Text.Json`  

---

## License

MIT License