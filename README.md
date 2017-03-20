# Project Kaleidscope

Project Kaleidscope is a modern, lightweight MediaWiki API wrapper for .NET platforms (.NET Core and .NET Framework) built by Moegirlpedia. It's goal is making interoperating with MediaWiki easy and enjoyable.

### Why?

The original MediaWiki API lives for years (even prior to modern technology). All actions call one single endpoint (`/api.php`), everything is described with query string or form data, which doesn't make sense for modern application development.

To address this issue, we developed this library to make development easier.

### Be careful!

This is an **experimental project**, which means lots of changes may happen every day. We are still working on making API calls more elegant.

### Get started

Check out the source tree, and build it with .NET Core SDK (at least 1.0 RTM).

### Technology

Project Kaleidscope utilizes [Dependency Injection](https://en.wikipedia.org/wiki/Dependency_injection). For example, if you want to use your own session provider module (e.g. JWT token), you may implement your own `ISessionProvider`, then inject your implementation to API service collection. Then all requests in this service collection scope will use JWT token for authentication instead of the old-school cookie-based one.

For more information about Dependency Injection, check out our wiki documentation and online resources.

### Get invovled

Feel free to open an issue if you encounter problems. For pull requests, all unit tests should pass.