# Middleware ASP.NET Core Auditoria 

Middleware de ASP.NET core para auditar las llamadas a los endpoint de una API REST. Este middleware captura los requests y response de los endpoint invocados llevadolos a json para luego ser persistidos según decisión técnica.

En este caso se incluye un proyecto adicional que integra con AWS SQS para delegar el procesamiento de la mensajería en otro componente que consuma dicha cola de mensaje.

## Características

- se permite excluir información de la mensajería indicando las propiedades a ignorar.
- parte de la información a ser registrar en la auditoría depende de la información contenida en el payload del token JWT.

