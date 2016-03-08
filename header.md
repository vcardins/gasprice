# Overview

The API is based on REST principles and uses the GET, POST, PUT, and DELETE HTTP verbs to perform various actions:

*	__POST__		Create a new object
*	__GET__			Retrieve an object or objects
*	__PUT__			Update an existing object
*	__DELETE__		Delete an existing object
*	__PATCH__		Update partially an existing object

The base URI for the API is located at https://api.results.com/api/v1:

The path below this URI includes the various objects you can perform actions on:
*   /businessunits 
*   /countries 
*   /goals 
*   /login 
*   /logout 
*   /newsfeeds 
*   /organizations 
*   /parkinglot 
*   /tasks 
*   /timezones 
*   /users

For example, the login endpoint is located at https://api/results.com/api/v2/login
Inputs to these methods (when applicable) are passed in as form post variables. This can be done using the ‘curl’ command line program. For example, to login:
```
curl https://api.results.com/api/v2/login -d "username=youruser@yourdomain.com" –d "password=yourpassword"
```

This will send a POST request to the URI specified with the username and password form fields set to the values specified. The response will be JSON-formatted. For the login method, it will look like:

```{ token: "12345678-ABCD-9876-5432-0123456789AB" }```

The token from the login method will be used to validate your credentials upon every successive call to the API. This is passed to the API as the HTTP header “X-Token”. For example, to retrieve the logged-in user information, you would perform:
```
curl https://api.results.com/api/v1/users/me -H "X-Token: 12345678-ABCD-9876-5432-0123456789AB"
```

The response from this call with again be a JSON response:
```{ "data": { "id": 1, "firstname": "John", "lastname": "Doe", "email": "johndoe@example.com", ... } }```
Note that the API may evolve to return additional fields which it does not presently. However, backwards compatibility will be maintained for all fields used in the POST / PUT (created/update) methods for as long as is feasible. Breaking changes will be avoided whenever possible.

### Status Codes

The API utilizes standard HTTP status codes to indicate the result of an operation. When an error occurs, the body of the response will generally include some amount of information concerning the error. The codes are as below:

*	__200 OK__ :		The GET/PUT/DELETE operation was successful.
*	__201 Created__: 		Used when POSTing a new item, this indicates that the object was successfully created. The URI of the new item will be returned in the “Location” header of the response, for example:
Location: /api/v1/tasks/1234
*	__400 Bad Request__			The input could not be validated, either due to a missing parameter or a bad value passed for the parameter.
*	__401 No Authorization__		This is returned if the “X-Token” header isn’t passed in, if the value passed in is malformed, or if it is rejected for security or
other reasons.
*	__403 Access Denied__		Returned when you do not have permission to access an object. For example, a task belonging to another user.
*	__404 Not Found__		Returned if there is no method available or if the requested object cannot be found. Due to security reasons, this is also used when an object by that ID may exist for another organization but is not in yours.
*	__403 Access Denied__		Returned when you do not have permission to access an object. For example, a task belonging to another user.
*	__406 Not Acceptable__		Returned when a business logic validation condition happens, such as when the input parameters supplied do not validate. The nature of the error will be returned in the body of the response.
*	__500 Internal Server Error__		An unexpected error has occurred that doesn’t fit into any of the above categories.


### How it is structured

Below the API has been divided into various sections according to functionality. Note that parameters to operations are specified using curly braces, such as:

```GET /tasks/{id}``` This indicates that id is a variable. If you wish to retrieve task 1234, the URI would be /tasks/1234 as the curly braces are omitted.

### Test Operations

Base URI: https://api.results.com/api/v2

Various test operations have been included as a sort of "sanity check" to ensure the API is functioning correctly.

```GET /``` : Return the path of the request which, for this version, will always return the text "/api/v1".
```GET /hello/{name}``` : Returns the string “Hello {name}”. For example, /hello/John returns "Hello John".
