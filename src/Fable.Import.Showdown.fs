// ts2fable 0.6.2
module rec Fable.Import.Showdown

open System
open Fable.Core
open Fable.Import.JS
open Fable.Import.Browser

let [<Import("*","showdown")>] showdown: Showdown.IExports = jsNative

module Showdown =

    type [<AllowNullLiteral>] IExports =
        abstract Converter: ConverterStatic
        abstract helper: Helper
        /// <summary>Setting a "global" option affects all instances of showdown</summary>
        /// <param name="optionKey"></param>
        /// <param name="value"></param>
        abstract setOption: optionKey: string * value: obj option -> unit
        /// <summary>Retrieve previous set global option.</summary>
        /// <param name="optionKey"></param>
        abstract getOption: optionKey: string -> obj option
        /// Retrieve previous set global options.
        abstract getOptions: unit -> ShowdownOptions
        /// Reset options.
        abstract resetOptions: unit -> unit
        /// Retrieve the default options.
        abstract getDefaultOptions: unit -> ShowdownOptions
        /// Registered extensions
        abstract extension: name: string * extension: U3<(unit -> ShowdownExtension), (unit -> ResizeArray<ShowdownExtension>), ShowdownExtension> -> unit
        abstract extensions: TypeLiteral_01
        /// Get an extension.
        abstract resetExtensions: unit -> unit
        abstract getAllExtensions: unit -> GetAllExtensionsReturn
        /// <summary>Remove an extension.</summary>
        /// <param name="name"></param>
        abstract removeExtension: name: string -> unit
        /// <summary>Setting a "global" flavor affects all instances of showdown</summary>
        /// <param name="name"></param>
        abstract setFlavor: name: U5<string, string, string, string, string> -> unit

    type [<AllowNullLiteral>] GetAllExtensionsReturn =
        [<Emit "$0[$1]{{=$2}}">] abstract Item: name: string -> ResizeArray<ShowdownExtension> with get, set

    type [<AllowNullLiteral>] Extension =
        /// Property defines the nature of said sub-extensions and can assume 2 values:
        ///
        /// * `lang` - Language extensions add new markdown syntax to showdown.
        /// * `output` - Output extensions (or modifiers) alter the HTML output generated by showdown
        abstract ``type``: string with get, set

    /// Regex/replace style extensions are very similar to javascript's string.replace function.
    /// Two properties are given, `regex` and `replace`.
    type [<AllowNullLiteral>] RegexReplaceExtension =
        inherit Extension
        /// Should be either a string or a RegExp object.
        ///
        /// Keep in mind that, if a string is used, it will automatically be given a g modifier,
        /// that is, it is assumed to be a global replacement.
        abstract regex: U2<string, RegExp> option with get, set
        /// Can be either a string or a function. If replace is a string,
        /// it can use the $1 syntax for group substitution,
        /// exactly as if it were making use of string.replace (internally it does this actually).
        abstract replace: obj option with get, set

    /// If you'd just like to do everything yourself,you can specify a filter property.
    /// The filter property should be a function that acts as a callback.
    type [<AllowNullLiteral>] FilterExtension =
        inherit Extension
        abstract filter: (string -> Converter -> ConverterOptions -> string) option with get, set

    /// Defines a plugin/extension
    /// Each single extension can be one of two types:
    ///
    /// + Language Extension -- Language extensions are ones that that add new markdown syntax to showdown. For example, say you wanted ^^youtube http://www.youtube.com/watch?v=oHg5SJYRHA0 to automatically render as an embedded YouTube video, that would be a language extension.
    /// + Output Modifiers -- After showdown has run, and generated HTML, an output modifier would change that HTML. For example, say you wanted to change <div class="header"> to be <header>, that would be an output modifier.
    ///
    /// Each extension can provide two combinations of interfaces for showdown.
    type [<AllowNullLiteral>] ShowdownExtension =
        inherit RegexReplaceExtension
        inherit FilterExtension

    type [<AllowNullLiteral>] ConverterExtensions =
        abstract language: ResizeArray<ShowdownExtension> with get, set
        abstract output: ResizeArray<ShowdownExtension> with get, set

    type [<AllowNullLiteral>] Metadata =
        [<Emit "$0[$1]{{=$2}}">] abstract Item: meta: string -> string with get, set

    type [<AllowNullLiteral>] ShowdownOptions =
        /// Omit the trailing newline in a code block. Ex:
        ///
        /// This:
        ///    <code><pre>var foo = 'bar';
        ///    </pre></code>
        ///
        /// Becomes this:
        ///    <code><pre>var foo = 'bar';</pre></code>
        abstract omitExtraWLInCodeBlocks: bool option with get, set
        /// Disable the automatic generation of header ids. Setting to true overrides <strong>prefixHeaderId</strong>.
        abstract noHeaderId: bool option with get, set
        /// Use text in curly braces as header id.
        abstract customizedHeaderId: bool option with get, set
        /// Generate header ids compatible with github style (spaces are replaced
        /// with dashes and a bunch of non alphanumeric chars are removed).
        abstract ghCompatibleHeaderId: bool option with get, set
        /// Add a prefix to the generated header ids.
        /// Passing a string will prefix that string to the header id.
        /// Setting to true will add a generic 'section' prefix.
        abstract prefixHeaderId: U2<string, bool> option with get, set
        /// Enable support for setting image dimensions from within markdown syntax.
        /// Examples:
        ///
        ///    ![foo](foo.jpg =100x80)     simple, assumes units are in px
        ///    ![bar](bar.jpg =100x*)      sets the height to "auto"
        ///    ![baz](baz.jpg =80%x5em)  Image with width of 80% and height of 5em
        abstract parseImgDimensions: bool option with get, set
        /// Set the header starting level. For instance, setting this to 3 means that
        ///
        ///    # foo
        ///
        /// will be parsed as
        ///
        ///    <h3>foo</h3>
        abstract headerLevelStart: float option with get, set
        /// Turning this option on will enable automatic linking to urls.
        abstract simplifiedAutoLink: bool option with get, set
        /// This option excludes trailing punctuation from autolinking urls.
        /// Punctuation excluded: . ! ? ( ). Only applies if simplifiedAutoLink option is set to true.
        abstract excludeTrailingPunctuationFromURLs: bool option with get, set
        /// Turning this on will stop showdown from interpreting underscores in the middle of
        /// words as <em> and <strong> and instead treat them as literal underscores.
        ///
        /// Example:
        ///
        ///    some text with__underscores__in middle
        ///
        /// will be parsed as
        ///
        ///    <p>some text with__underscores__in middle</p>
        abstract literalMidWordUnderscores: bool option with get, set
        /// Enable support for strikethrough syntax.
        /// `~~strikethrough~~` as `<del>strikethrough</del>`.
        abstract strikethrough: bool option with get, set
        /// Enable support for tables syntax. Example:
        ///
        ///    | h1    |    h2   |      h3 |
        ///    |:------|:-------:|--------:|
        ///    | 100   | [a][1]  | ![b][2] |
        ///    | *foo* | **bar** | ~~baz~~ |
        ///
        /// See the wiki for more info
        abstract tables: bool option with get, set
        /// If enabled adds an id property to table headers tags.
        abstract tablesHeaderId: bool option with get, set
        /// Enable support for GFM code block style.
        abstract ghCodeBlocks: bool option with get, set
        /// Enable support for GFM takslists. Example:
        ///
        ///    - [x] This task is done
        ///    - [ ] This is still pending
        abstract tasklists: bool option with get, set
        /// Prevents weird effects in live previews due to incomplete input.
        abstract smoothLivePreview: bool option with get, set
        /// Tries to smartly fix indentation problems related to es6 template
        /// strings in the midst of indented code.
        abstract smartIndentationFix: bool option with get, set
        /// Disables the requirement of indenting sublists by 4 spaces for them to be nested,
        /// effectively reverting to the old behavior where 2 or 3 spaces were enough.
        abstract disableForced4SpacesIndentedSublists: bool option with get, set
        /// Parses line breaks as like GitHub does, without needing 2 spaces at the end of the line.
        abstract simpleLineBreaks: bool option with get, set
        /// Makes adding a space between # and the header text mandatory.
        abstract requireSpaceBeforeHeadingText: bool option with get, set
        /// Enables github @mentions, which link to the username mentioned
        abstract ghMentions: bool option with get, set
        /// Changes the link generated by @mentions. Showdown will replace {u}
        /// with the username. Only applies if ghMentions option is enabled.
        /// Example: @tivie with ghMentionsOption set to //mysite.com/{u}/profile will
        /// result in <a href="//mysite.com/tivie/profile">@tivie</a>
        abstract ghMentionsLink: string option with get, set
        /// Open all links in new windows (by adding the attribute target="_blank" to <a> tags).
        abstract openLinksInNewWindow: bool option with get, set
        /// Support for HTML Tag escaping. ex: \<div>foo\</div>.
        abstract backslashEscapesHTMLTags: bool option with get, set
        /// Enable emoji support. Ex: `this is a :smile: emoji.
        abstract emoji: bool option with get, set
        /// Enable support for underline. Syntax is double or triple underscores: `__underline word__`. With this option enabled,
        /// underscores no longer parses into `<em>` and `<strong>`
        abstract underline: bool option with get, set
        /// Outputs a complete html document, including `<html>`, `<head>` and `<body>` tags
        abstract completeHTMLDocument: bool option with get, set
        /// Outputs a complete html document, including `<html>`, `<head>` and `<body>` tags
        abstract metadata: bool option with get, set
        /// Split adjacent blockquote blocks
        abstract splitAdjacentBlockquotes: bool option with get, set

    type [<AllowNullLiteral>] ConverterOptions =
        inherit ShowdownOptions
        abstract extensions: U2<string, ResizeArray<string>> option with get, set

    /// Constructor function for a Converter
    type [<AllowNullLiteral>] Converter =
        /// <param name="text">The input text (markdown)</param>
        abstract makeHtml: text: string -> string
        /// <summary>Converts an HTML string into a markdown string</summary>
        /// <param name="src">The input text (HTML)</param>
        /// <param name="HTMLParser">A WHATWG DOM and HTML parser, such as JSDOM. If none is supplied, window.document will be used.</param>
        abstract makeMarkdown: src: string * ?HTMLParser: HTMLDocument -> string
        /// <summary>Setting a "local" option only affects the specified Converter object.</summary>
        /// <param name="optionKey"></param>
        /// <param name="value"></param>
        abstract setOption: optionKey: string * value: obj option -> unit
        /// <summary>Get the option of this Converter instance.</summary>
        /// <param name="optionKey"></param>
        abstract getOption: optionKey: string -> obj option
        /// Get the options of this Converter instance.
        abstract getOptions: unit -> ShowdownOptions
        /// <summary>Add extension to THIS converter.</summary>
        /// <param name="extension"></param>
        /// <param name="name"></param>
        abstract addExtension: extension: ShowdownExtension * name: string -> unit
        abstract addExtension: extension: ResizeArray<ShowdownExtension> * name: string -> unit
        /// <summary>Use a global registered extension with THIS converter</summary>
        /// <param name="extensionName">Name of the previously registered extension.</param>
        abstract useExtension: extensionName: string -> unit
        /// Get all extensions.
        abstract getAllExtensions: unit -> ConverterExtensions
        /// <summary>Remove an extension from THIS converter.
        ///
        /// Note: This is a costly operation. It's better to initialize a new converter
        /// and specify the extensions you wish to use.</summary>
        /// <param name="extensions"></param>
        abstract removeExtension: extensions: U2<ResizeArray<ShowdownExtension>, ShowdownExtension> -> unit
        /// Set a "local" flavor for THIS Converter instance
        abstract setFlavor: name: U5<string, string, string, string, string> -> unit
        /// <summary>Get the metadata of the previously parsed document</summary>
        /// <param name="raw"></param>
        abstract getMetadata: ?raw: bool -> U2<string, Metadata>
        /// Get the metadata format of the previously parsed document
        abstract getMetadataFormat: unit -> string



    type [<AllowNullLiteral>] ConverterStatic =
        /// <param name="converterOptions">Configuration object, describes which extensions to apply</param>
        [<Emit "new $0($1...)">] abstract Create: ?converterOptions: ConverterOptions -> Converter

    /// Helper Interface
    type [<AllowNullLiteral>] Helper =
        abstract replaceRecursiveRegExp: [<ParamArray>] args: ResizeArray<obj option> -> string

    type [<AllowNullLiteral>] TypeLiteral_01 =
        [<Emit "$0[$1]{{=$2}}">] abstract Item: name: string -> ShowdownExtension with get, set
