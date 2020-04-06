// ==================================================
// Copyright 2019(C) , DotLogix
// File:  MimeTypes.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  15.08.2018
// LastEdited:  14.02.2019
// ==================================================

#region
#endregion

#region
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
#endregion

namespace DotLogix.Core.Rest.Server.Http.Headers {
    public static class MimeTypes {
        private static readonly ConcurrentDictionary<string, string> ExtensionMapping = new ConcurrentDictionary<string, string>();

        static MimeTypes() {
            AddMapping("application/atom+xml", ".atom");
            AddMapping("application/directx", ".x");
            AddMapping("application/envoy", ".evy");
            AddMapping("application/fractals", ".fif");
            AddMapping("application/futuresplash", ".spl");
            AddMapping("application/hta", ".hta");
            AddMapping("application/internet-property-stream", ".acx");
            AddMapping("application/java-archive", ".jar");
            AddMapping("application/liquidmotion", ".jck");
            AddMapping("application/liquidmotion", ".jcz");
            AddMapping("application/mac-binhex40", ".hqx");
            AddMapping("application/msaccess", ".accdb");
            AddMapping("application/msaccess", ".accde");
            AddMapping("application/msaccess", ".accdt");
            AddMapping("application/msword", ".doc");
            AddMapping("application/msword", ".dot");
            AddMapping("application/octet-stream", ".aaf");
            AddMapping("application/octet-stream", ".aca");
            AddMapping("application/octet-stream", ".afm");
            AddMapping("application/octet-stream", ".asd");
            AddMapping("application/octet-stream", ".asi");
            AddMapping("application/octet-stream", ".bin");
            AddMapping("application/octet-stream", ".cab");
            AddMapping("application/octet-stream", ".chm");
            AddMapping("application/octet-stream", ".csv");
            AddMapping("application/octet-stream", ".cur");
            AddMapping("application/octet-stream", ".deploy");
            AddMapping("application/octet-stream", ".dsp");
            AddMapping("application/octet-stream", ".dwp");
            AddMapping("application/octet-stream", ".emz");
            AddMapping("application/octet-stream", ".eot");
            AddMapping("application/octet-stream", ".exe");
            AddMapping("application/octet-stream", ".fla");
            AddMapping("application/octet-stream", ".hhk");
            AddMapping("application/octet-stream", ".hhp");
            AddMapping("application/octet-stream", ".ics");
            AddMapping("application/octet-stream", ".inf");
            AddMapping("application/octet-stream", ".java");
            AddMapping("application/octet-stream", ".jpb");
            AddMapping("application/octet-stream", ".lpk");
            AddMapping("application/octet-stream", ".lzh");
            AddMapping("application/octet-stream", ".mdp");
            AddMapping("application/octet-stream", ".mix");
            AddMapping("application/octet-stream", ".msi");
            AddMapping("application/octet-stream", ".mso");
            AddMapping("application/octet-stream", ".ocx");
            AddMapping("application/octet-stream", ".pcx");
            AddMapping("application/octet-stream", ".pcz");
            AddMapping("application/octet-stream", ".pfb");
            AddMapping("application/octet-stream", ".pfm");
            AddMapping("application/octet-stream", ".prm");
            AddMapping("application/octet-stream", ".prx");
            AddMapping("application/octet-stream", ".psd");
            AddMapping("application/octet-stream", ".psm");
            AddMapping("application/octet-stream", ".psp");
            AddMapping("application/octet-stream", ".qxd");
            AddMapping("application/octet-stream", ".rar");
            AddMapping("application/octet-stream", ".sea");
            AddMapping("application/octet-stream", ".smi");
            AddMapping("application/octet-stream", ".snp");
            AddMapping("application/octet-stream", ".thn");
            AddMapping("application/octet-stream", ".toc");
            AddMapping("application/octet-stream", ".ttf");
            AddMapping("application/octet-stream", ".u32");
            AddMapping("application/octet-stream", ".xsn");
            AddMapping("application/octet-stream", ".xtp");
            AddMapping("application/oda", ".oda");
            AddMapping("application/oleobject", ".ods");
            AddMapping("application/olescript", ".axs");
            AddMapping("application/onenote", ".one");
            AddMapping("application/onenote", ".onea");
            AddMapping("application/onenote", ".onepkg");
            AddMapping("application/onenote", ".onetmp");
            AddMapping("application/onenote", ".onetoc");
            AddMapping("application/onenote", ".onetoc2");
            AddMapping("application/opensearchdescription+xml", ".osdx");
            AddMapping("application/pdf", ".pdf");
            AddMapping("application/pics-rules", ".prf");
            AddMapping("application/pkcs10", ".p10");
            AddMapping("application/pkcs7-mime", ".p7c");
            AddMapping("application/pkcs7-mime", ".p7m");
            AddMapping("application/pkcs7-signature", ".p7s");
            AddMapping("application/pkix-crl", ".crl");
            AddMapping("application/postscript", ".ai");
            AddMapping("application/postscript", ".eps");
            AddMapping("application/postscript", ".ps");
            AddMapping("application/rtf", ".rtf");
            AddMapping("application/set-payment-initiation", ".setpay");
            AddMapping("application/set-registration-initiation", ".setreg");
            AddMapping("application/streamingmedia", ".ssm");
            AddMapping("application/vnd.fdf", ".fdf");
            AddMapping("application/vnd.ms-excel", ".xla");
            AddMapping("application/vnd.ms-excel", ".xlc");
            AddMapping("application/vnd.ms-excel", ".xlm");
            AddMapping("application/vnd.ms-excel", ".xls");
            AddMapping("application/vnd.ms-excel", ".xlt");
            AddMapping("application/vnd.ms-excel", ".xlw");
            AddMapping("application/vnd.ms-excel.addin.macroEnabled.12", ".xlam");
            AddMapping("application/vnd.ms-excel.sheet.binary.macroEnabled.12", ".xlsb");
            AddMapping("application/vnd.ms-excel.sheet.macroEnabled.12", ".xlsm");
            AddMapping("application/vnd.ms-excel.template.macroEnabled.12", ".xltm");
            AddMapping("application/vnd.ms-office.calx", ".calx");
            AddMapping("application/vnd.ms-officetheme", ".thmx");
            AddMapping("application/vnd.ms-pki.certstore", ".sst");
            AddMapping("application/vnd.ms-pki.pko", ".pko");
            AddMapping("application/vnd.ms-pki.seccat", ".cat");
            AddMapping("application/vnd.ms-pki.stl", ".stl");
            AddMapping("application/vnd.ms-powerpoint", ".pot");
            AddMapping("application/vnd.ms-powerpoint", ".pps");
            AddMapping("application/vnd.ms-powerpoint", ".ppt");
            AddMapping("application/vnd.ms-powerpoint.addin.macroEnabled.12", ".ppam");
            AddMapping("application/vnd.ms-powerpoint.presentation.macroEnabled.12", ".pptm");
            AddMapping("application/vnd.ms-powerpoint.slide.macroEnabled.12", ".sldm");
            AddMapping("application/vnd.ms-powerpoint.slideshow.macroEnabled.12", ".ppsm");
            AddMapping("application/vnd.ms-powerpoint.template.macroEnabled.12", ".potm");
            AddMapping("application/vnd.ms-project", ".mpp");
            AddMapping("application/vnd.ms-visio.viewer", ".vdx");
            AddMapping("application/vnd.ms-word.document.macroEnabled.12", ".docm");
            AddMapping("application/vnd.ms-word.template.macroEnabled.12", ".dotm");
            AddMapping("application/vnd.ms-works", ".wcm");
            AddMapping("application/vnd.ms-works", ".wdb");
            AddMapping("application/vnd.ms-works", ".wks");
            AddMapping("application/vnd.ms-works", ".wps");
            AddMapping("application/vnd.ms-xpsdocument", ".xps");
            AddMapping("application/vnd.openxmlformats-officedocument.presentationml.presentation", ".pptx");
            AddMapping("application/vnd.openxmlformats-officedocument.presentationml.slide", ".sldx");
            AddMapping("application/vnd.openxmlformats-officedocument.presentationml.slideshow", ".ppsx");
            AddMapping("application/vnd.openxmlformats-officedocument.presentationml.template", ".potx");
            AddMapping("application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", ".xlsx");
            AddMapping("application/vnd.openxmlformats-officedocument.spreadsheetml.template", ".xltx");
            AddMapping("application/vnd.openxmlformats-officedocument.wordprocessingml.document", ".docx");
            AddMapping("application/vnd.openxmlformats-officedocument.wordprocessingml.template", ".dotx");
            AddMapping("application/vnd.rn-realmedia", ".rm");
            AddMapping("application/vnd.visio", ".vsd");
            AddMapping("application/vnd.visio", ".vss");
            AddMapping("application/vnd.visio", ".vst");
            AddMapping("application/vnd.visio", ".vsw");
            AddMapping("application/vnd.visio", ".vsx");
            AddMapping("application/vnd.visio", ".vtx");
            AddMapping("application/vnd.wap.wmlc", ".wmlc");
            AddMapping("application/vnd.wap.wmlscriptc", ".wmlsc");
            AddMapping("application/winhlp", ".hlp");
            AddMapping("application/x-bcpio", ".bcpio");
            AddMapping("application/x-cdf", ".cdf");
            AddMapping("application/x-compress", ".z");
            AddMapping("application/x-compressed", ".tgz");
            AddMapping("application/x-cpio", ".cpio");
            AddMapping("application/x-csh", ".csh");
            AddMapping("application/x-director", ".dcr");
            AddMapping("application/x-director", ".dir");
            AddMapping("application/x-director", ".dxr");
            AddMapping("application/x-dvi", ".dvi");
            AddMapping("application/x-gtar", ".gtar");
            AddMapping("application/x-gzip", ".gz");
            AddMapping("application/x-hdf", ".hdf");
            AddMapping("application/x-internet-signup", ".ins");
            AddMapping("application/x-internet-signup", ".isp");
            AddMapping("application/x-iphone", ".iii");
            AddMapping("application/x-java-applet", ".class");
            AddMapping("application/x-javascript", ".js");
            AddMapping("application/x-latex", ".latex");
            AddMapping("application/x-miva-compiled", ".mvc");
            AddMapping("application/x-ms-application", ".application");
            AddMapping("application/x-ms-manifest", ".manifest");
            AddMapping("application/x-ms-reader", ".lit");
            AddMapping("application/x-ms-vsto", ".vsto");
            AddMapping("application/x-ms-wmd", ".wmd");
            AddMapping("application/x-ms-wmz", ".wmz");
            AddMapping("application/x-ms-xbap", ".xbap");
            AddMapping("application/x-msaccess", ".mdb");
            AddMapping("application/x-mscardfile", ".crd");
            AddMapping("application/x-msclip", ".clp");
            AddMapping("application/x-msdownload", ".dll");
            AddMapping("application/x-msmediaview", ".m13");
            AddMapping("application/x-msmediaview", ".m14");
            AddMapping("application/x-msmediaview", ".mvb");
            AddMapping("application/x-msmetafile", ".wmf");
            AddMapping("application/x-msmoney", ".mny");
            AddMapping("application/x-mspublisher", ".pub");
            AddMapping("application/x-msschedule", ".scd");
            AddMapping("application/x-msterminal", ".trm");
            AddMapping("application/x-mswrite", ".wri");
            AddMapping("application/x-netcdf", ".nc");
            AddMapping("application/x-oleobject", ".hhc");
            AddMapping("application/x-perfmon", ".pma");
            AddMapping("application/x-perfmon", ".pmc");
            AddMapping("application/x-perfmon", ".pml");
            AddMapping("application/x-perfmon", ".pmr");
            AddMapping("application/x-perfmon", ".pmw");
            AddMapping("application/x-pkcs12", ".p12");
            AddMapping("application/x-pkcs12", ".pfx");
            AddMapping("application/x-pkcs7-certificates", ".p7b");
            AddMapping("application/x-pkcs7-certificates", ".spc");
            AddMapping("application/x-pkcs7-certreqresp", ".p7r");
            AddMapping("application/x-quicktimeplayer", ".qtl");
            AddMapping("application/x-sh", ".sh");
            AddMapping("application/x-shar", ".shar");
            AddMapping("application/x-shockwave-flash", ".swf");
            AddMapping("application/x-silverlight-app", ".xap");
            AddMapping("application/x-smaf", ".mmf");
            AddMapping("application/x-stuffit", ".sit");
            AddMapping("application/x-sv4cpio", ".sv4cpio");
            AddMapping("application/x-sv4crc", ".sv4crc");
            AddMapping("application/x-tar", ".tar");
            AddMapping("application/x-tcl", ".tcl");
            AddMapping("application/x-tex", ".tex");
            AddMapping("application/x-texinfo", ".texi");
            AddMapping("application/x-texinfo", ".texinfo");
            AddMapping("application/x-troff", ".roff");
            AddMapping("application/x-troff", ".t");
            AddMapping("application/x-troff", ".tr");
            AddMapping("application/x-troff-man", ".man");
            AddMapping("application/x-troff-me", ".me");
            AddMapping("application/x-troff-ms", ".ms");
            AddMapping("application/x-ustar", ".ustar");
            AddMapping("application/x-wais-source", ".src");
            AddMapping("application/x-x509-ca-cert", ".crt");
            AddMapping("application/x-x509-ca-cert", ".der");
            AddMapping("application/x-zip-compressed", ".zip");
            AddMapping("application/xaml+xml", ".xaml");
            AddMapping("audio/aiff", ".aifc");
            AddMapping("audio/aiff", ".aiff");
            AddMapping("audio/basic", ".au");
            AddMapping("audio/basic", ".snd");
            AddMapping("audio/mid", ".mid");
            AddMapping("audio/mid", ".midi");
            AddMapping("audio/mid", ".rmi");
            AddMapping("audio/mpeg", ".mp3");
            AddMapping("audio/wav", ".wav");
            AddMapping("audio/x-aiff", ".aif");
            AddMapping("audio/x-mpegurl", ".m3u");
            AddMapping("audio/x-ms-wax", ".wax");
            AddMapping("audio/x-ms-wma", ".wma");
            AddMapping("audio/x-pn-realaudio", ".ra");
            AddMapping("audio/x-pn-realaudio", ".ram");
            AddMapping("audio/x-pn-realaudio-plugin", ".rpm");
            AddMapping("audio/x-smd", ".smd");
            AddMapping("audio/x-smd", ".smx");
            AddMapping("audio/x-smd", ".smz");
            AddMapping("drawing/x-dwf", ".dwf");
            AddMapping("image/bmp", ".bmp");
            AddMapping("image/bmp", ".dib");
            AddMapping("image/cis-cod", ".cod");
            AddMapping("image/gif", ".gif");
            AddMapping("image/ief", ".ief");
            AddMapping("image/jpeg", ".jpe");
            AddMapping("image/jpeg", ".jpeg");
            AddMapping("image/jpeg", ".jpg");
            AddMapping("image/pjpeg", ".jfif");
            AddMapping("image/png", ".png");
            AddMapping("image/png", ".pnz");
            AddMapping("image/tiff", ".tif");
            AddMapping("image/tiff", ".tiff");
            AddMapping("image/vnd.rn-realflash", ".rf");
            AddMapping("image/vnd.wap.wbmp", ".wbmp");
            AddMapping("image/x-cmu-raster", ".ras");
            AddMapping("image/x-cmx", ".cmx");
            AddMapping("image/x-icon", ".ico");
            AddMapping("image/x-jg", ".art");
            AddMapping("image/x-portable-anymap", ".pnm");
            AddMapping("image/x-portable-bitmap", ".pbm");
            AddMapping("image/x-portable-graymap", ".pgm");
            AddMapping("image/x-portable-pixmap", ".ppm");
            AddMapping("image/x-rgb", ".rgb");
            AddMapping("image/x-xbitmap", ".xbm");
            AddMapping("image/x-xpixmap", ".xpm");
            AddMapping("image/x-xwindowdump", ".xwd");
            AddMapping("message/rfc822", ".eml");
            AddMapping("message/rfc822", ".mht");
            AddMapping("message/rfc822", ".mhtml");
            AddMapping("message/rfc822", ".nws");
            AddMapping("text/css", ".css");
            AddMapping("text/dlm", ".dlm");
            AddMapping("text/h323", ".323");
            AddMapping("text/html", ".htm");
            AddMapping("text/html", ".html");
            AddMapping("text/html", ".hxt");
            AddMapping("text/iuls", ".uls");
            AddMapping("text/jscript", ".jsx");
            AddMapping("text/plain", ".asm");
            AddMapping("text/plain", ".bas");
            AddMapping("text/plain", ".c");
            AddMapping("text/plain", ".cnf");
            AddMapping("text/plain", ".cpp");
            AddMapping("text/plain", ".h");
            AddMapping("text/plain", ".map");
            AddMapping("text/plain", ".txt");
            AddMapping("text/plain", ".vcs");
            AddMapping("text/plain", ".xdr");
            AddMapping("text/richtext", ".rtx");
            AddMapping("text/scriptlet", ".sct");
            AddMapping("text/sgml", ".sgml");
            AddMapping("text/tab-separated-values", ".tsv");
            AddMapping("text/vbscript", ".vbs");
            AddMapping("text/vnd.wap.wml", ".wml");
            AddMapping("text/vnd.wap.wmlscript", ".wmls");
            AddMapping("text/webviewhtml", ".htt");
            AddMapping("text/x-component", ".htc");
            AddMapping("text/x-hdml", ".hdml");
            AddMapping("text/x-ms-odc", ".odc");
            AddMapping("text/x-setext", ".etx");
            AddMapping("text/x-vcard", ".vcf");
            AddMapping("text/xml", ".disco");
            AddMapping("text/xml", ".dll.config");
            AddMapping("text/xml", ".dtd");
            AddMapping("text/xml", ".exe.config");
            AddMapping("text/xml", ".mno");
            AddMapping("text/xml", ".vml");
            AddMapping("text/xml", ".wsdl");
            AddMapping("text/xml", ".xml");
            AddMapping("text/xml", ".xsd");
            AddMapping("text/xml", ".xsf");
            AddMapping("text/xml", ".xsl");
            AddMapping("text/xml", ".xslt");
            AddMapping("video/mpeg", ".m1v");
            AddMapping("video/mpeg", ".mp2");
            AddMapping("video/mpeg", ".mpa");
            AddMapping("video/mpeg", ".mpe");
            AddMapping("video/mpeg", ".mpeg");
            AddMapping("video/mpeg", ".mpg");
            AddMapping("video/mpeg", ".mpv2");
            AddMapping("video/quicktime", ".mov");
            AddMapping("video/quicktime", ".qt");
            AddMapping("video/x-flv", ".flv");
            AddMapping("video/x-ivf", ".IVF");
            AddMapping("video/x-la-asf", ".lsf");
            AddMapping("video/x-la-asf", ".lsx");
            AddMapping("video/x-ms-asf", ".asf");
            AddMapping("video/x-ms-asf", ".asr");
            AddMapping("video/x-ms-asf", ".asx");
            AddMapping("video/x-ms-asf", ".nsc");
            AddMapping("video/x-ms-wm", ".wm");
            AddMapping("video/x-ms-wmp", ".wmp");
            AddMapping("video/x-ms-wmv", ".wmv");
            AddMapping("video/x-ms-wmx", ".wmx");
            AddMapping("video/x-ms-wvx", ".wvx");
            AddMapping("video/x-msvideo", ".avi");
            AddMapping("video/x-sgi-movie", ".movie");
            AddMapping("x-world/x-vrml", ".flr");
            AddMapping("x-world/x-vrml", ".wrl");
            AddMapping("x-world/x-vrml", ".wrz");
            AddMapping("x-world/x-vrml", ".xaf");
            AddMapping("x-world/x-vrml", ".xof");
        }

        public static MimeType GetByExtension(string extension) {
            return ExtensionMapping.TryGetValue(extension, out var mimeType) ? new MimeType(mimeType) : null;
        }

        public static bool TryGetByExtension(string extension, out MimeType mimeType) {
            if(ExtensionMapping.TryGetValue(extension, out var value)) {
                mimeType = new MimeType(value);
                return true;
            }

            mimeType = default(MimeType);
            return false;
        }

        public static void AddMapping(string mimeType, string extension) {
            ExtensionMapping[extension] = mimeType;
        }

        public static void AddMapping(string mimeType, params string[] extensions) {
            foreach(var extension in extensions)
                AddMapping(mimeType, extension);
        }

        public static void AddMapping(string mimeType, IEnumerable<string> extensions) {
            foreach(var extension in extensions)
                AddMapping(mimeType, extension);
        }


        public static void AddMapping(MimeType mimeType, string extension) {
            AddMapping(mimeType.Value, extension);
        }

        public static void AddMapping(MimeType mimeType, params string[] extensions) {
            foreach(var extension in extensions)
                AddMapping(mimeType.Value, extension);
        }

        public static void AddMapping(MimeType mimeType, IEnumerable<string> extensions) {
            foreach(var extension in extensions)
                AddMapping(mimeType.Value, extension);
        }

        public static void RemoveMapping(string extension) {
            ExtensionMapping.TryRemove(extension, out _);
        }

        public static void RemoveMapping(params string[] extensions) {
            RemoveMapping(extensions.AsEnumerable());
        }

        public static void RemoveMapping(IEnumerable<string> extensions) {
            foreach(var extension in extensions)
                ExtensionMapping.TryRemove(extension, out _);
        }


        public static class Application {
            public static MimeType Gzip => new MimeType("application/gzip");
            public static MimeType JavaScript => new MimeType("application/javascript");
            public static MimeType Json => new MimeType("application/json");
            public static MimeType OctetStream => new MimeType("application/octet-stream");
            public static MimeType Pdf => new MimeType("application/pdf");
            public static MimeType PostScript => new MimeType("application/postscript");
            public static MimeType Rtc => new MimeType("application/rtc");
            public static MimeType Rtf => new MimeType("application/rtf");
            public static MimeType XHtml => new MimeType("application/xhtml+xml");
            public static MimeType Xml => new MimeType("application/xml");
            public static MimeType Latex => new MimeType("application/x-latex");
            public static MimeType Tar => new MimeType("application/x-tar");
            public static MimeType Zip => new MimeType("application/zip");
        }

        public static class Audio {
            public static MimeType Aiff => new MimeType("audio/x-aiff");
            public static MimeType Midi => new MimeType("audio/x-midi");
            public static MimeType Mpeg => new MimeType("audio/x-mpeg");
            public static MimeType QuickTimeStream => new MimeType("audio/x-qt-stream");
            public static MimeType Wav => new MimeType("audio/x-wav");
        }

        public static class MsOffice {
            public static MimeType Doc => new MimeType("application/msword");
            public static MimeType Dot => new MimeType("application/msword");
            public static MimeType Docx => new MimeType("application/vnd.openxmlformats-officedocument.wordprocessingml.document");
            public static MimeType Dotx => new MimeType("application/vnd.openxmlformats-officedocument.wordprocessingml.template");
            public static MimeType Docm => new MimeType("application/vnd.ms-word.document.macroEnabled.12");
            public static MimeType Dotm => new MimeType("application/vnd.ms-word.template.macroEnabled.12");
            public static MimeType Xls => new MimeType("application/vnd.ms-excel");
            public static MimeType Xlt => new MimeType("application/vnd.ms-excel");
            public static MimeType Xla => new MimeType("application/vnd.ms-excel");
            public static MimeType Xlsx => new MimeType("application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
            public static MimeType Xltx => new MimeType("application/vnd.openxmlformats-officedocument.spreadsheetml.template");
            public static MimeType Xlsm => new MimeType("application/vnd.ms-excel.sheet.macroEnabled.12");
            public static MimeType Xltm => new MimeType("application/vnd.ms-excel.template.macroEnabled.12");
            public static MimeType Xlam => new MimeType("application/vnd.ms-excel.addin.macroEnabled.12");
            public static MimeType Xlsb => new MimeType("application/vnd.ms-excel.sheet.binary.macroEnabled.12");
            public static MimeType Ppt => new MimeType("application/vnd.ms-powerpoint");
            public static MimeType Pot => new MimeType("application/vnd.ms-powerpoint");
            public static MimeType Pps => new MimeType("application/vnd.ms-powerpoint");
            public static MimeType Ppa => new MimeType("application/vnd.ms-powerpoint");
            public static MimeType Pptx => new MimeType("application/vnd.openxmlformats-officedocument.presentationml.presentation");
            public static MimeType Potx => new MimeType("application/vnd.openxmlformats-officedocument.presentationml.template");
            public static MimeType Ppsx => new MimeType("application/vnd.openxmlformats-officedocument.presentationml.slideshow");
            public static MimeType Ppam => new MimeType("application/vnd.ms-powerpoint.addin.macroEnabled.12");
            public static MimeType Pptm => new MimeType("application/vnd.ms-powerpoint.presentation.macroEnabled.12");
            public static MimeType Potm => new MimeType("application/vnd.ms-powerpoint.template.macroEnabled.12");
            public static MimeType Ppsm => new MimeType("application/vnd.ms-powerpoint.slideshow.macroEnabled.12");
            public static MimeType Mdb => new MimeType("application/vnd.ms-access");
        }

        public static class Image {
            public static MimeType Bmp => new MimeType("image/bmp");
            public static MimeType Gif => new MimeType("image/gif");
            public static MimeType Jpeg => new MimeType("image/jpeg");
            public static MimeType Png => new MimeType("image/png");
            public static MimeType Svg => new MimeType("image/svg+xml");
            public static MimeType Tiff => new MimeType("image/tiff");
            public static MimeType Icon => new MimeType("image/x-icon");
        }

        public static class Text {
            public static MimeType Csv => new MimeType("text/comma-separated-values");
            public static MimeType Css => new MimeType("text/css");
            public static MimeType Html => new MimeType("text/html");
            public static MimeType JavaScript => new MimeType("text/javascript");
            public static MimeType Plain => new MimeType("text/plain");
            public static MimeType RichText => new MimeType("text/richtext");
            public static MimeType Rtf => new MimeType("text/rtf");
            public static MimeType TabSeparatedValues => new MimeType("text/tab-separated-values");
            public static MimeType Xml => new MimeType("text/xml");
        }

        public static class Video {
            public static MimeType Mpeg => new MimeType("video/mpeg");
            public static MimeType QuickTime => new MimeType("video/quicktime");
            public static MimeType Avi => new MimeType("video/x-msvideo");
        }
    }
}
