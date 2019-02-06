// ==================================================
// Copyright 2018(C) , DotLogix
// File:  MimeTypes.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  17.02.2018
// LastEdited:  01.08.2018
// ==================================================

#region
#endregion

namespace DotLogix.Core.Rest.Server.Http.Headers {
    public static class MimeTypes {
        public static class Application {
            public static MimeType Gzip { get; } = new MimeType("application/gzip");
            public static MimeType JavaScript { get; } = new MimeType("application/javascript");
            public static MimeType Json { get; } = new MimeType("application/json");
            public static MimeType OctetStream { get; } = new MimeType("application/octet-stream");
            public static MimeType Pdf { get; } = new MimeType("application/pdf");
            public static MimeType PostScript { get; } = new MimeType("application/postscript");
            public static MimeType Rtc { get; } = new MimeType("application/rtc");
            public static MimeType Rtf { get; } = new MimeType("application/rtf");
            public static MimeType XHtml { get; } = new MimeType("application/xhtml+xml");
            public static MimeType Xml { get; } = new MimeType("application/xml");
            public static MimeType Latex { get; } = new MimeType("application/x-latex");
            public static MimeType Tar { get; } = new MimeType("application/x-tar");
            public static MimeType Zip { get; } = new MimeType("application/zip");
        }

        public static class Audio {
            public static MimeType Aiff { get; } = new MimeType("audio/x-aiff");
            public static MimeType Midi { get; } = new MimeType("audio/x-midi");
            public static MimeType Mpeg { get; } = new MimeType("audio/x-mpeg");
            public static MimeType QuickTimeStream { get; } = new MimeType("audio/x-qt-stream");
            public static MimeType Wav { get; } = new MimeType("audio/x-wav");
        }

        public static class MsOffice {
            public static MimeType Doc { get; } = new MimeType("application/msword");
            public static MimeType Dot { get; } = new MimeType("application/msword");
            public static MimeType Docx { get; } = new MimeType("application/vnd.openxmlformats-officedocument.wordprocessingml.document");
            public static MimeType Dotx { get; } = new MimeType("application/vnd.openxmlformats-officedocument.wordprocessingml.template");
            public static MimeType Docm { get; } = new MimeType("application/vnd.ms-word.document.macroEnabled.12");
            public static MimeType Dotm { get; } = new MimeType("application/vnd.ms-word.template.macroEnabled.12");
            public static MimeType Xls { get; } = new MimeType("application/vnd.ms-excel");
            public static MimeType Xlt { get; } = new MimeType("application/vnd.ms-excel");
            public static MimeType Xla { get; } = new MimeType("application/vnd.ms-excel");
            public static MimeType Xlsx { get; } = new MimeType("application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
            public static MimeType Xltx { get; } = new MimeType("application/vnd.openxmlformats-officedocument.spreadsheetml.template");
            public static MimeType Xlsm { get; } = new MimeType("application/vnd.ms-excel.sheet.macroEnabled.12");
            public static MimeType Xltm { get; } = new MimeType("application/vnd.ms-excel.template.macroEnabled.12");
            public static MimeType Xlam { get; } = new MimeType("application/vnd.ms-excel.addin.macroEnabled.12");
            public static MimeType Xlsb { get; } = new MimeType("application/vnd.ms-excel.sheet.binary.macroEnabled.12");
            public static MimeType Ppt { get; } = new MimeType("application/vnd.ms-powerpoint");
            public static MimeType Pot { get; } = new MimeType("application/vnd.ms-powerpoint");
            public static MimeType Pps { get; } = new MimeType("application/vnd.ms-powerpoint");
            public static MimeType Ppa { get; } = new MimeType("application/vnd.ms-powerpoint");
            public static MimeType Pptx { get; } = new MimeType("application/vnd.openxmlformats-officedocument.presentationml.presentation");
            public static MimeType Potx { get; } = new MimeType("application/vnd.openxmlformats-officedocument.presentationml.template");
            public static MimeType Ppsx { get; } = new MimeType("application/vnd.openxmlformats-officedocument.presentationml.slideshow");
            public static MimeType Ppam { get; } = new MimeType("application/vnd.ms-powerpoint.addin.macroEnabled.12");
            public static MimeType Pptm { get; } = new MimeType("application/vnd.ms-powerpoint.presentation.macroEnabled.12");
            public static MimeType Potm { get; } = new MimeType("application/vnd.ms-powerpoint.template.macroEnabled.12");
            public static MimeType Ppsm { get; } = new MimeType("application/vnd.ms-powerpoint.slideshow.macroEnabled.12");
            public static MimeType Mdb { get; } = new MimeType("application/vnd.ms-access");
        }

        public static class Image {
            public static MimeType Bmp { get; } = new MimeType("image/bmp");
            public static MimeType Gif { get; } = new MimeType("image/gif");
            public static MimeType Jpeg { get; } = new MimeType("image/jpeg");
            public static MimeType Png { get; } = new MimeType("image/png");
            public static MimeType Svg { get; } = new MimeType("image/svg+xml");
            public static MimeType Tiff { get; } = new MimeType("image/tiff");
            public static MimeType Icon { get; } = new MimeType("image/x-icon");
        }

        public static class Text {
            public static MimeType Csv { get; } = new MimeType("text/comma-separated-values");
            public static MimeType Css { get; } = new MimeType("text/css");
            public static MimeType Html { get; } = new MimeType("text/html");
            public static MimeType JavaScript { get; } = new MimeType("text/javascript");
            public static MimeType Plain { get; } = new MimeType("text/plain");
            public static MimeType RichText { get; } = new MimeType("text/richtext");
            public static MimeType Rtf { get; } = new MimeType("text/rtf");
            public static MimeType TabSeparatedValues { get; } = new MimeType("text/tab-separated-values");
            public static MimeType Xml { get; } = new MimeType("text/xml");
        }

        public static class Video {
            public static MimeType Mpeg { get; } = new MimeType("video/mpeg");
            public static MimeType QuickTime { get; } = new MimeType("video/quicktime");
            public static MimeType Avi { get; } = new MimeType("video/x-msvideo");
        }
    }
}
