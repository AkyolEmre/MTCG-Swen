# Monster Trading Cards Game (MTCG)

## Einleitung
Dieses Projekt implementiert ein Monster Trading Cards Game (MTCG) mit einem HTTP-Server, der Benutzern ermöglicht, sich zu registrieren, anzumelden und Kartenkämpfe durchzuführen.

## Voraussetzungen
- .NET 6.0 SDK: [Download .NET 6.0 SDK](https://dotnet.microsoft.com/download/dotnet/6.0)
- Visual Studio Code oder Visual Studio
- PlantUML und Graphviz (optional für UML-Diagramme)

## Installation
1. **Klonen des Repositories**
    ```bash
    git clone https://github.com/AkyolEmre/MTCG-Swen.git
    cd MTCG-Swen
    ```

2. **Abhängigkeiten installieren**
    ```bash
    dotnet restore
    ```

## Ausführen des Projekts
1. **Visual Studio Code öffnen**
    - Öffne Visual Studio Code und lade das geklonte Repository.

2. **Starten des Servers**
    - Öffne die `Program.cs`-Datei und stelle sicher, dass die `Main`-Methode korrekt auf deinen `HttpSvr` zeigt.
    - Starte das Projekt mit `F5` oder durch Klicken auf das grüne Play-Symbol in Visual Studio Code.

3. **Testen der Endpunkte**
    - Verwende Postman oder cURL, um die Endpunkte zu testen.
    - Beispiel für die Registrierung eines neuen Benutzers:
        ```bash
        curl -X POST http://localhost:12000/register -d "username=testuser&password=1234"
        ```
    - Beispiel für das Anmelden eines Benutzers:
        ```bash
        curl -X POST http://localhost:12000/login -d "username=testuser&password=1234"
        ```

## UML-Diagramme erstellen (optional)
1. **PlantUML und Graphviz installieren**
    - PlantUML Extension für Visual Studio Code installieren
    - Graphviz installieren: [Download Graphviz](https://graphviz.gitlab.io/_pages/Download/Download_windows.html)

2. **UML-Diagramm generieren**
    - Erstelle eine Datei `diagram.puml` und füge deinen PlantUML-Code hinzu.
    - Öffne die Datei in Visual Studio Code und drücke `Alt + D`, um das Diagramm anzuzeigen.

## Github Repository Link
[Link zum Repository](https://github.com/AkyolEmre/MTCG-Swen)

## Nächste Schritte
- Implementierung der Logik für spezielle Kartenfähigkeiten und deren Auswirkungen im Kampf.
- Integration von Sicherheitsmechanismen zur Sicherstellung der Datenintegrität.
- Unit-Tests für die einzelnen Komponenten, um die Stabilität und Funktionalität des Systems zu überprüfen.

## Autor
- **Akyol Emre**
