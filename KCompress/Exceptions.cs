/*  This file is part of KCompressSharp.

    KCompressSharp is free software: you can redistribute it and/or modify
    it under the terms of the GNU Lesser General Public License as published by
    the Free Software Foundation, either version 3 of the License, or
    (at your option) any later version.

    KCompressSharp is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    GNU Lesser General Public License for more details.

    You should have received a copy of the GNU Lesser General Public License
    along with KCompressSharp.  If not, see <http://www.gnu.org/licenses/>.
*/

using System;
#if !WINCE
using System.Runtime.Serialization;
#endif

namespace KCompress
{    
    /// <summary>
    /// Base KCompress exception class.
    /// </summary>
    [Serializable]
    public class KCompressException : Exception
    {
        /// <summary>
        /// The message for thrown user exceptions.
        /// </summary>
        internal const string USER_EXCEPTION_MESSAGE = "The extraction was successful but" +
            "some exceptions were thrown in your events. Check UserExceptions for details.";

        /// <summary>
        /// Initializes a new instance of the KCompressException class
        /// </summary>
        public KCompressException() : base("KCompress unknown exception.") {}

        /// <summary>
        /// Initializes a new instance of the KCompressException class
        /// </summary>
        /// <param name="defaultMessage">Default exception message</param>
        public KCompressException(string defaultMessage)
            : base(defaultMessage) {}

        /// <summary>
        /// Initializes a new instance of the KCompressException class
        /// </summary>
        /// <param name="defaultMessage">Default exception message</param>
        /// <param name="message">Additional detailed message</param>
        public KCompressException(string defaultMessage, string message)
            : base(defaultMessage + " Message: " + message) {}

        /// <summary>
        /// Initializes a new instance of the KCompressException class
        /// </summary>
        /// <param name="defaultMessage">Default exception message</param>
        /// <param name="message">Additional detailed message</param>
        /// <param name="inner">Inner exception occured</param>
        public KCompressException(string defaultMessage, string message, Exception inner)
            : base(
                defaultMessage + (defaultMessage.EndsWith(" ", StringComparison.CurrentCulture) ? "" : " Message: ") +
                message, inner) {}

        /// <summary>
        /// Initializes a new instance of the KCompressException class
        /// </summary>
        /// <param name="defaultMessage">Default exception message</param>
        /// <param name="inner">Inner exception occured</param>
        public KCompressException(string defaultMessage, Exception inner)
            : base(defaultMessage, inner) {}
#if !WINCE
        /// <summary>
        /// Initializes a new instance of the KCompressException class
        /// </summary>
        /// <param name="info">All data needed for serialization or deserialization</param>
        /// <param name="context">Serialized stream descriptor</param>
        protected KCompressException(
            SerializationInfo info, StreamingContext context)
            : base(info, context) {}
#endif
    }

#if UNMANAGED
    
    /// <summary>
    /// Exception class for ArchiveExtractCallback.
    /// </summary>
    [Serializable]
    public class ExtractionFailedException : KCompressException
    {
        /// <summary>
        /// Exception dafault message which is displayed if no extra information is specified
        /// </summary>
        public const string DEFAULT_MESSAGE = "Could not extract files!";

        /// <summary>
        /// Initializes a new instance of the ExtractionFailedException class
        /// </summary>
        public ExtractionFailedException() : base(DEFAULT_MESSAGE) {}

        /// <summary>
        /// Initializes a new instance of the ExtractionFailedException class
        /// </summary>
        /// <param name="message">Additional detailed message</param>
        public ExtractionFailedException(string message) : base(DEFAULT_MESSAGE, message) {}

        /// <summary>
        /// Initializes a new instance of the ExtractionFailedException class
        /// </summary>
        /// <param name="message">Additional detailed message</param>
        /// <param name="inner">Inner exception occured</param>
        public ExtractionFailedException(string message, Exception inner) : base(DEFAULT_MESSAGE, message, inner) {}
#if !WINCE
        /// <summary>
        /// Initializes a new instance of the ExtractionFailedException class
        /// </summary>
        /// <param name="info">All data needed for serialization or deserialization</param>
        /// <param name="context">Serialized stream descriptor</param>
        protected ExtractionFailedException(
            SerializationInfo info, StreamingContext context)
            : base(info, context) {}
#endif
    }

#if COMPRESS
    
    /// <summary>
    /// Exception class for ArchiveUpdateCallback.
    /// </summary>
    [Serializable]
    public class CompressionFailedException : KCompressException
    {
        /// <summary>
        /// Exception dafault message which is displayed if no extra information is specified
        /// </summary>
        public const string DEFAULT_MESSAGE = "Could not pack files!";

        /// <summary>
        /// Initializes a new instance of the CompressionFailedException class
        /// </summary>
        public CompressionFailedException() : base(DEFAULT_MESSAGE) {}

        /// <summary>
        /// Initializes a new instance of the CompressionFailedException class
        /// </summary>
        /// <param name="message">Additional detailed message</param>
        public CompressionFailedException(string message) : base(DEFAULT_MESSAGE, message) {}

        /// <summary>
        /// Initializes a new instance of the CompressionFailedException class
        /// </summary>
        /// <param name="message">Additional detailed message</param>
        /// <param name="inner">Inner exception occured</param>
        public CompressionFailedException(string message, Exception inner) : base(DEFAULT_MESSAGE, message, inner) {}
#if !WINCE
        /// <summary>
        /// Initializes a new instance of the CompressionFailedException class
        /// </summary>
        /// <param name="info">All data needed for serialization or deserialization</param>
        /// <param name="context">Serialized stream descriptor</param>
        protected CompressionFailedException(
            SerializationInfo info, StreamingContext context)
            : base(info, context) {}
#endif
    }
#endif
#endif
    
    /// <summary>
    /// Exception class for LZMA operations.
    /// </summary>
    [Serializable]
    public class LzmaException : KCompressException
    {
        /// <summary>
        /// Exception dafault message which is displayed if no extra information is specified
        /// </summary>
        public const string DEFAULT_MESSAGE = "Specified stream is not a valid LZMA compressed stream!";

        /// <summary>
        /// Initializes a new instance of the LzmaException class
        /// </summary>
        public LzmaException() : base(DEFAULT_MESSAGE) {}

        /// <summary>
        /// Initializes a new instance of the LzmaException class
        /// </summary>
        /// <param name="message">Additional detailed message</param>
        public LzmaException(string message) : base(DEFAULT_MESSAGE, message) {}

        /// <summary>
        /// Initializes a new instance of the LzmaException class
        /// </summary>
        /// <param name="message">Additional detailed message</param>
        /// <param name="inner">Inner exception occured</param>
        public LzmaException(string message, Exception inner) : base(DEFAULT_MESSAGE, message, inner) {}
#if !WINCE
        /// <summary>
        /// Initializes a new instance of the LzmaException class
        /// </summary>
        /// <param name="info">All data needed for serialization or deserialization</param>
        /// <param name="context">Serialized stream descriptor</param>
        protected LzmaException(
            SerializationInfo info, StreamingContext context)
            : base(info, context) {}
#endif
    }

#if UNMANAGED
    
    /// <summary>
    /// Exception class for 7-zip archive open or read operations.
    /// </summary>
    [Serializable]
    public class KCompressArchiveException : KCompressException
    {
        /// <summary>
        /// Exception dafault message which is displayed if no extra information is specified
        /// </summary>
        public const string DEFAULT_MESSAGE =
            "Invalid archive: open/read error! Is it encrypted and a wrong password was provided?\n" +
            "If your archive is an exotic one, it is possible that KCompressSharp has no signature for "+
            "its format and thus decided it is TAR by mistake.";

        /// <summary>
        /// Initializes a new instance of the KCompressArchiveException class
        /// </summary>
        public KCompressArchiveException() : base(DEFAULT_MESSAGE) {}

        /// <summary>
        /// Initializes a new instance of the KCompressArchiveException class
        /// </summary>
        /// <param name="message">Additional detailed message</param>
        public KCompressArchiveException(string message) : base(DEFAULT_MESSAGE, message) {}

        /// <summary>
        /// Initializes a new instance of the KCompressArchiveException class
        /// </summary>
        /// <param name="message">Additional detailed message</param>
        /// <param name="inner">Inner exception occured</param>
        public KCompressArchiveException(string message, Exception inner) : base(DEFAULT_MESSAGE, message, inner) {}
#if !WINCE
        /// <summary>
        /// Initializes a new instance of the KCompressArchiveException class
        /// </summary>
        /// <param name="info">All data needed for serialization or deserialization</param>
        /// <param name="context">Serialized stream descriptor</param>
        protected KCompressArchiveException(
            SerializationInfo info, StreamingContext context)
            : base(info, context) {}
#endif
    }
    
    /// <summary>
    /// Exception class for empty common root if file name array in KCompressCompressor.
    /// </summary>
    [Serializable]
    public class KCompressInvalidFileNamesException : KCompressException
    {
        /// <summary>
        /// Exception dafault message which is displayed if no extra information is specified
        /// </summary>
        public const string DEFAULT_MESSAGE = "Invalid file names have been specified: ";

        /// <summary>
        /// Initializes a new instance of the KCompressInvalidFileNamesException class
        /// </summary>
        public KCompressInvalidFileNamesException() : base(DEFAULT_MESSAGE) {}

        /// <summary>
        /// Initializes a new instance of the KCompressInvalidFileNamesException class
        /// </summary>
        /// <param name="message">Additional detailed message</param>
        public KCompressInvalidFileNamesException(string message) : base(DEFAULT_MESSAGE, message) {}

        /// <summary>
        /// Initializes a new instance of the KCompressInvalidFileNamesException class
        /// </summary>
        /// <param name="message">Additional detailed message</param>
        /// <param name="inner">Inner exception occured</param>
        public KCompressInvalidFileNamesException(string message, Exception inner) : base(DEFAULT_MESSAGE, message, inner) {}
#if !WINCE
        /// <summary>
        /// Initializes a new instance of the KCompressInvalidFileNamesException class
        /// </summary>
        /// <param name="info">All data needed for serialization or deserialization</param>
        /// <param name="context">Serialized stream descriptor</param>
        protected KCompressInvalidFileNamesException(
            SerializationInfo info, StreamingContext context)
            : base(info, context) {}
#endif
    }

#if COMPRESS
    
    /// <summary>
    /// Exception class for fail to create an archive in KCompressCompressor.
    /// </summary>
    [Serializable]
    public class KCompressCompressionFailedException : KCompressException
    {
        /// <summary>
        /// Exception dafault message which is displayed if no extra information is specified
        /// </summary>
        public const string DEFAULT_MESSAGE = "The compression has failed for an unknown reason with code ";

        /// <summary>
        /// Initializes a new instance of the KCompressCompressionFailedException class
        /// </summary>
        public KCompressCompressionFailedException() : base(DEFAULT_MESSAGE) {}

        /// <summary>
        /// Initializes a new instance of the KCompressCompressionFailedException class
        /// </summary>
        /// <param name="message">Additional detailed message</param>
        public KCompressCompressionFailedException(string message) : base(DEFAULT_MESSAGE, message) {}

        /// <summary>
        /// Initializes a new instance of the KCompressCompressionFailedException class
        /// </summary>
        /// <param name="message">Additional detailed message</param>
        /// <param name="inner">Inner exception occured</param>
        public KCompressCompressionFailedException(string message, Exception inner)
            : base(DEFAULT_MESSAGE, message, inner) {}
#if !WINCE
        /// <summary>
        /// Initializes a new instance of the KCompressCompressionFailedException class
        /// </summary>
        /// <param name="info">All data needed for serialization or deserialization</param>
        /// <param name="context">Serialized stream descriptor</param>
        protected KCompressCompressionFailedException(
            SerializationInfo info, StreamingContext context)
            : base(info, context) {}
#endif
    }
#endif
    
    /// <summary>
    /// Exception class for fail to extract an archive in KCompressExtractor.
    /// </summary>
    [Serializable]
    public class KCompressExtractionFailedException : KCompressException
    {
        /// <summary>
        /// Exception dafault message which is displayed if no extra information is specified
        /// </summary>
        public const string DEFAULT_MESSAGE = "The extraction has failed for an unknown reason with code ";

        /// <summary>
        /// Initializes a new instance of the KCompressExtractionFailedException class
        /// </summary>
        public KCompressExtractionFailedException() : base(DEFAULT_MESSAGE) {}

        /// <summary>
        /// Initializes a new instance of the KCompressExtractionFailedException class
        /// </summary>
        /// <param name="message">Additional detailed message</param>
        public KCompressExtractionFailedException(string message) : base(DEFAULT_MESSAGE, message) {}

        /// <summary>
        /// Initializes a new instance of the KCompressExtractionFailedException class
        /// </summary>
        /// <param name="message">Additional detailed message</param>
        /// <param name="inner">Inner exception occured</param>
        public KCompressExtractionFailedException(string message, Exception inner) : base(DEFAULT_MESSAGE, message, inner) {}
#if !WINCE
        /// <summary>
        /// Initializes a new instance of the KCompressExtractionFailedException class
        /// </summary>
        /// <param name="info">All data needed for serialization or deserialization</param>
        /// <param name="context">Serialized stream descriptor</param>
        protected KCompressExtractionFailedException(
            SerializationInfo info, StreamingContext context)
            : base(info, context) {}
#endif
    }
    
    /// <summary>
    /// Exception class for 7-zip library operations.
    /// </summary>
    [Serializable]
    public class KCompressLibraryException : KCompressException
    {
        /// <summary>
        /// Exception dafault message which is displayed if no extra information is specified
        /// </summary>
        public const string DEFAULT_MESSAGE = "Can not load 7-zip library or internal COM error!";

        /// <summary>
        /// Initializes a new instance of the KCompressLibraryException class
        /// </summary>
        public KCompressLibraryException() : base(DEFAULT_MESSAGE) {}

        /// <summary>
        /// Initializes a new instance of the KCompressLibraryException class
        /// </summary>
        /// <param name="message">Additional detailed message</param>
        public KCompressLibraryException(string message) : base(DEFAULT_MESSAGE, message) {}

        /// <summary>
        /// Initializes a new instance of the KCompressLibraryException class
        /// </summary>
        /// <param name="message">Additional detailed message</param>
        /// <param name="inner">Inner exception occured</param>
        public KCompressLibraryException(string message, Exception inner) : base(DEFAULT_MESSAGE, message, inner) {}
#if !WINCE
        /// <summary>
        /// Initializes a new instance of the KCompressLibraryException class
        /// </summary>
        /// <param name="info">All data needed for serialization or deserialization</param>
        /// <param name="context">Serialized stream descriptor</param>
        protected KCompressLibraryException(
            SerializationInfo info, StreamingContext context)
            : base(info, context) {}
#endif
    }
#endif

#if SFX
    
    /// <summary>
    /// Exception class for 7-zip sfx settings validation.
    /// </summary>
    [Serializable]
    public class KCompressSfxValidationException : KCompressException
    {
        /// <summary>
        /// Exception dafault message which is displayed if no extra information is specified
        /// </summary>
        public static readonly string DefaultMessage = "Sfx settings validation failed.";

        /// <summary>
        /// Initializes a new instance of the KCompressSfxValidationException class
        /// </summary>
        public KCompressSfxValidationException() : base(DefaultMessage) {}

        /// <summary>
        /// Initializes a new instance of the KCompressSfxValidationException class
        /// </summary>
        /// <param name="message">Additional detailed message</param>
        public KCompressSfxValidationException(string message) : base(DefaultMessage, message) {}

        /// <summary>
        /// Initializes a new instance of the KCompressSfxValidationException class
        /// </summary>
        /// <param name="message">Additional detailed message</param>
        /// <param name="inner">Inner exception occured</param>
        public KCompressSfxValidationException(string message, Exception inner) : base(DefaultMessage, message, inner) {}
#if !WINCE
        /// <summary>
        /// Initializes a new instance of the KCompressSfxValidationException class
        /// </summary>
        /// <param name="info">All data needed for serialization or deserialization</param>
        /// <param name="context">Serialized stream descriptor</param>
        protected KCompressSfxValidationException(
            SerializationInfo info, StreamingContext context)
            : base(info, context) {}
#endif
    }
#endif
}