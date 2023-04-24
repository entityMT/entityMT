## EntityMT

### What is?

Entity MT is a multi-tenant and multi-DBMS data persistence component. It was developed using SOLID principles, design patterns and component segregation best practices.

### Structure (UML components diagram)

![Alt text](./Uml/components.png)

. __Tenant (Abstraction) and Tenant (Implementation):__ Responsible for providing the current tenant and the respective connection string for the DBMS. <br/>
. __Configuration (Abstraction) and Configuration (Implementation):__ Responsible for providing the necessary settings for generating SQL queries and commands from/to Objects.
