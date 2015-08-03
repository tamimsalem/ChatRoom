# ChatRoom
This is a small solution that provides a sample for a small chat room application based on SignalR.

It's intended mainly to showcase the following:

- Live messaging using SignalR.
- Message Persistence using Azure Table Storage.
- Inversion of Control in backend using Autofac.
- Automatic deploy to azure cloud services using Powershell.

Evaluated Inversion of Control containers where the following:

- Castle Windsor
- Autofac
- Unity
- MEF (if considered an IoC container)

Reasons to go with Autofac instead of others:

- Official support for SignalR and MVC.
- Feature rich, with extendion points.
- Actively maintained.

The rest are either old, lacking features, or lacking certain flexibilties.

- Castle Windsor, has a minimal support for MVC that is more complicated that Autofac, also it doesn't support SignalR hubs.
- Unity is old, lacks features and not updated for a while.
- MEF, isn't an IoC container exactly, and therefore it lacks certainn features and flexibilities (choosing from multiple exports for example)

