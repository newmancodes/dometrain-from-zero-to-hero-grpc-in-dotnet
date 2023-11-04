# From Zero to Hero : gRPC in .NET - [Dometrain](https://app.dometrain.com/courses/from-zero-to-hero-grpc-in-net)

## Protocol Buffers

- Google developed language and platform-neutral data serialisation format
- Defines structure of types and operations
- Strongly typed
- Binary encoded
- Backward and forward compatible
- Extensible
- Multiple language support (C#, Go, Python, ...)

### .proto file

- The contract between consumers and server
- Authored in Protocol Buffers
- Defines message types and available operations on the server
- Compiled into language specific code and data types

```proto
syntax = "proto3";
option csharp_namespace = "FirstGrpc`;  

package greet;

message HelloRequest {
    string name = 1;
}

message HelloReply {
   string message = 1;
}

service Greeter {
    rpc SayHello (HellReuqest) returns (HelloReply);
}
```

### Scalar Types

- double
- float
- int32
- int64
- uint32
- uint64
- sint32
- sint64
- bool
- string
- bytes

- fixed32
- fixed64
- sfixed32
- sfixed64

### Enums

```proto
enum PhoneType {
    UNSPECIFIED = 0;
    HOME = 1;
    WORK = 2;
}
```

### Well-known types

Useful datatypes published by Google that are frequently needed.

```proto
syntax = "proto3";

import "google/protobuf/Empty.proto";

option csharp_namespace = "Service.Protos";
package service;

service MyServiceDefinition {
    rpc GetAll(google.protobuf.Empty) returns (strram Response) {}
}
```

#### Nullable types

```proto
syntax = "proto3";

import "google/protobuf/wrappers.proto";

message Person {
    google.protobuf.Int32Value age = 5;
}
```

|C# Type|Well-known type wrapper|
|---|---|
|double?|google.protobuf.DoubleValue|
|flaot?|google.protobuf.FloatValue|
|int?|google.protobuf.Int32Value|
|long?|google.protobuf.Int64Value|
|uint?|google.protobuf.UInt32Value|
|ulong?|google.protobuf.UInt64Value|

### Any

```proto
syntax = "proto3";

package tutorial;

import "google/protobuf/any.proto";

message ACat {
    string name = 1;
    int32 aage = 2;
    google.protobuf.Any care_giver=3;
}

message Owner {
    int32 id=1;
    string FirstName=2;
    string LastName=3;
}

message Foster {
    int32 id=1;
    string Address=2;
}
```

```csharp
var cat = new ACat();
cat.Name = "Tom";
cat.Age = 10;
cat.CareGiver = Any.Pack(new Owner(){
    FirstName = "John",
    LastName = "Doe",
    Id = 213213
});


void CheckCareGiver(ACat cat)
{
    if (cat.CareGiver.Is(Owner.Descriptor))
    {
        var owner = cat.CareGiver.Unpack<Owner>();
        Console.WriteLine($"Owner: {owner.FirstName} {owner.LastName}");
    }
    else if (cat.CareGiver.Is(Foster.Descriptor))
    {
        var foster = cat.CareGiver.Unpack<Foster>();
        Console.WriteLine($"Foster: {foster.Address}");
    }
}
```

### OneOf

Discriminated Union

```proto
import "google/protobuf/struct.proto";

package tutorial;

option csharp_namespace = "MyExample.Cat";

message Cat {
    google.protobug.Int32Value id=1;
    string name=2;
    NullableField profile_picture=3;
}

message NullableField {
    oneof kind {
        google.protobuf.NullValue null=1;
        google.protobuf.StringValue value=2;
    }
}
```

### Collections

Uses the `repeated` keyword

```proto
syntax = "proto3";
package tutorial;

import "google/protobuf/timestamp.proto";
option csarp_namespace = "MyExample.AddressBook";

message Person {
    string name = 1;
    int32 id = 2;
    string emai = 3;

    enum PhoneType {
        MOBILE = 0;
        HOME = 1;
        WORK = 2;
    }

    message PhoneNumber {
        string number = 1;
        PhoneType type = 2;
    }

    repeated PhoneNumber phones = 4;
    google.protobuf.Timestamp last_updated = 5;
}

message AddressBook {
    repeated Person people = 1;
}
```

### Method Types

- Unary (Request/Response)
- Client Streaming
- Server Streaming
- Bi-directional Streaming

## Advanced Features

### Interceptors

Available on both client and server.

### Compression

Enable compression and then use client metadata entry grpc-accept-encoding set to the appropriate algorithm e.g. gzip

### Transient Fault Handling

Can leverage retry policies which are transparent to the client or [hedging](https://grpc.io/docs/guides/request-hedging/).  
Can only pick one of these strategies, we have to choose the right tool for the job.

## Security

- WS-Federation
- JWT
- AAD / Entra Id
- Identity Server
- OAuth 2.0
- OpenID Connect
- Channel authentication
- Client certificate

Does not support Windows Authentication

Security can be applied at the:

- Call-level
- Channel-level

Can also use both together.
