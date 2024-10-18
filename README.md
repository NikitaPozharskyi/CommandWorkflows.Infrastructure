# CommandWorkflows.Infrastructure
This library provides tools for handling sequential user requests in non-sequential systems, such as Telegram bots or HTTP-based applications. 
It allows for extending functionalities with minimal overhead, offering reusable workflows that can be shared across multiple commands.

Technically, the library utilizes in-memory state to track and recognize previous user requests in its default implementation. 
It also provides interfaces for external communication, enabling sequential request handling to be managed outside the application state using various storage mechanisms.

This solution is ideal for scenarios where predefined actions need to be performed based on user interactions, leveraging workflows to streamline the process. 
It integrates seamlessly with dependency injection (DI) to register commands and workflows, ensuring they are injected into the appropriate places within the request handler. 
Commands can be defined and workflows attached to them via ServiceProvider extension methods, establishing a strict, ordered execution of system actions based on user input.

exaple of using this library can be found in TestApplication
