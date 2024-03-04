![Motoko](Html/Images/Motoko.webp)

# VSpace Encoding

VSpace Encoding is a novel text encoding scheme designed to securely encode sensitive information into a seemingly invisible format using ASCII whitespace characters. 
This project aims to protect textual data such as cryptographic keys or personal information from being easily captured 
through screen photographs or casual observation. By leveraging basic ASCII whitespace for encoding, 
VSpace ensures compatibility across a wide range of environments, including those outside of Unicode editors.

Current implementations include a C# and a JavaScript version. 
The JavaScript implementation has an accompanying HTML page for demonstration purposes.
The C# version includes unit tests, which the JavaScript version does not.

## Features

- **Stealthy Text Encoding**: Encodes text into a format using ASCII whitespace characters, making the encoded information invisible in casual observation and immune to screen capture.
- **Cross-Platform Compatibility**: Utilizes ASCII characters for broad compatibility across different systems and environments, requiring no special software or Unicode support.
- **Simple Integration**: Designed to be easily integrated into various applications, offering a straightforward API for encoding and decoding operations.
- **Enhanced Security**: Provides an additional layer of security for sensitive information, ideal for protecting data in less secure or public environments.

## Example Applications

* Secure Note Sharing: Encode sensitive notes before sharing through email or messaging apps.
* Crypto Key Management: Safely store cryptographic keys in text files or cloud storage without plain text visibility.
* Privacy in Collaborative Environments: Keep sensitive information hidden while working in shared or public coding spaces.

## Live Sample

See [this live sample](https://danielvaughan.org/Samples/VSpaceEncoding/) to view the JavaScript implementation in action.

## Getting Started

Clone this repository to get started:

```bash
git clone https://github.com/DVaughan/VSpaceEncoding.git
```

### Usage

#### Encoding Text

To encode a piece of text, use the Encode method:

C#
```cs
AsciiVSpaceEncoder encoder = new();
string secretMessage = "Your Secret Text Here";
string encodedText = encoder.Encode(secretMessage);
```

JavaScript
```js
var encoder = new AsciiVSpaceEncoder();
var secretMessage = "Your Secret Text Here";
var encodedText = encoder.encode(secretMessage);
```

#### Decoding Text

To decode previously encoded text, use the Decode method:

C#
```cs
string decodedText = encoder.Decode(encodedText);
```

JavaScript
```js
var text = encoder.decode(encodedText);
```

## Contributing

We welcome contributions to improve VSpace Encoding. If you have a feature request, bug report, or a patch, please feel free to submit an issue or pull request on GitHub.

## License

Distributed under the MIT License. See LICENSE for more information.

## Attributions

This work is based on my (Daniel Vaughan) work on [Invisible Ink](https://github.com/DVaughan/InvisibleInk), which provides a similar mechanism
for encoding text, but which includes encryption and is designed for Unicode environments.