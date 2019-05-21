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
            AddMapping("text/h323", ".323");
            AddMapping("application/octet-stream", ".aaf");
            AddMapping("application/octet-stream", ".aca");
            AddMapping("application/msaccess", ".accdb");
            AddMapping("application/msaccess", ".accde");
            AddMapping("application/msaccess", ".accdt");
            AddMapping("application/internet-property-stream", ".acx");
            AddMapping("application/octet-stream", ".afm");
            AddMapping("application/postscript", ".ai");
            AddMapping("audio/x-aiff", ".aif");
            AddMapping("audio/aiff", ".aifc");
            AddMapping("audio/aiff", ".aiff");
            AddMapping("application/x-ms-application", ".application");
            AddMapping("image/x-jg", ".art");
            AddMapping("application/octet-stream", ".asd");
            AddMapping("video/x-ms-asf", ".asf");
            AddMapping("application/octet-stream", ".asi");
            AddMapping("text/plain", ".asm");
            AddMapping("video/x-ms-asf", ".asr");
            AddMapping("video/x-ms-asf", ".asx");
            AddMapping("application/atom+xml", ".atom");
            AddMapping("audio/basic", ".au");
            AddMapping("video/x-msvideo", ".avi");
            AddMapping("application/olescript", ".axs");
            AddMapping("text/plain", ".bas");
            AddMapping("application/x-bcpio", ".bcpio");
            AddMapping("application/octet-stream", ".bin");
            AddMapping("image/bmp", ".bmp");
            AddMapping("text/plain", ".c");
            AddMapping("application/octet-stream", ".cab");
            AddMapping("application/vnd.ms-office.calx", ".calx");
            AddMapping("application/vnd.ms-pki.seccat", ".cat");
            AddMapping("application/x-cdf", ".cdf");
            AddMapping("application/octet-stream", ".chm");
            AddMapping("application/x-java-applet", ".class");
            AddMapping("application/x-msclip", ".clp");
            AddMapping("image/x-cmx", ".cmx");
            AddMapping("text/plain", ".cnf");
            AddMapping("image/cis-cod", ".cod");
            AddMapping("application/x-cpio", ".cpio");
            AddMapping("text/plain", ".cpp");
            AddMapping("application/x-mscardfile", ".crd");
            AddMapping("application/pkix-crl", ".crl");
            AddMapping("application/x-x509-ca-cert", ".crt");
            AddMapping("application/x-csh", ".csh");
            AddMapping("text/css", ".css");
            AddMapping("application/octet-stream", ".csv");
            AddMapping("application/octet-stream", ".cur");
            AddMapping("application/x-director", ".dcr");
            AddMapping("application/octet-stream", ".deploy");
            AddMapping("application/x-x509-ca-cert", ".der");
            AddMapping("image/bmp", ".dib");
            AddMapping("application/x-director", ".dir");
            AddMapping("text/xml", ".disco");
            AddMapping("application/x-msdownload", ".dll");
            AddMapping("text/xml", ".dll.config");
            AddMapping("text/dlm", ".dlm");
            AddMapping("application/msword", ".doc");
            AddMapping("application/vnd.ms-word.document.macroEnabled.12", ".docm");
            AddMapping("application/vnd.openxmlformats-officedocument.wordprocessingml.document", ".docx");
            AddMapping("application/msword", ".dot");
            AddMapping("application/vnd.ms-word.template.macroEnabled.12", ".dotm");
            AddMapping("application/vnd.openxmlformats-officedocument.wordprocessingml.template", ".dotx");
            AddMapping("application/octet-stream", ".dsp");
            AddMapping("text/xml", ".dtd");
            AddMapping("application/x-dvi", ".dvi");
            AddMapping("drawing/x-dwf", ".dwf");
            AddMapping("application/octet-stream", ".dwp");
            AddMapping("application/x-director", ".dxr");
            AddMapping("message/rfc822", ".eml");
            AddMapping("application/octet-stream", ".emz");
            AddMapping("application/octet-stream", ".eot");
            AddMapping("application/postscript", ".eps");
            AddMapping("text/x-setext", ".etx");
            AddMapping("application/envoy", ".evy");
            AddMapping("application/octet-stream", ".exe");
            AddMapping("text/xml", ".exe.config");
            AddMapping("application/vnd.fdf", ".fdf");
            AddMapping("application/fractals", ".fif");
            AddMapping("application/octet-stream", ".fla");
            AddMapping("x-world/x-vrml", ".flr");
            AddMapping("video/x-flv", ".flv");
            AddMapping("image/gif", ".gif");
            AddMapping("application/x-gtar", ".gtar");
            AddMapping("application/x-gzip", ".gz");
            AddMapping("text/plain", ".h");
            AddMapping("application/x-hdf", ".hdf");
            AddMapping("text/x-hdml", ".hdml");
            AddMapping("application/x-oleobject", ".hhc");
            AddMapping("application/octet-stream", ".hhk");
            AddMapping("application/octet-stream", ".hhp");
            AddMapping("application/winhlp", ".hlp");
            AddMapping("application/mac-binhex40", ".hqx");
            AddMapping("application/hta", ".hta");
            AddMapping("text/x-component", ".htc");
            AddMapping("text/html", ".htm");
            AddMapping("text/html", ".html");
            AddMapping("text/webviewhtml", ".htt");
            AddMapping("text/html", ".hxt");
            AddMapping("image/x-icon", ".ico");
            AddMapping("application/octet-stream", ".ics");
            AddMapping("image/ief", ".ief");
            AddMapping("application/x-iphone", ".iii");
            AddMapping("application/octet-stream", ".inf");
            AddMapping("application/x-internet-signup", ".ins");
            AddMapping("application/x-internet-signup", ".isp");
            AddMapping("video/x-ivf", ".IVF");
            AddMapping("application/java-archive", ".jar");
            AddMapping("application/octet-stream", ".java");
            AddMapping("application/liquidmotion", ".jck");
            AddMapping("application/liquidmotion", ".jcz");
            AddMapping("image/pjpeg", ".jfif");
            AddMapping("application/octet-stream", ".jpb");
            AddMapping("image/jpeg", ".jpe");
            AddMapping("image/jpeg", ".jpeg");
            AddMapping("image/jpeg", ".jpg");
            AddMapping("application/x-javascript", ".js");
            AddMapping("text/jscript", ".jsx");
            AddMapping("application/x-latex", ".latex");
            AddMapping("application/x-ms-reader", ".lit");
            AddMapping("application/octet-stream", ".lpk");
            AddMapping("video/x-la-asf", ".lsf");
            AddMapping("video/x-la-asf", ".lsx");
            AddMapping("application/octet-stream", ".lzh");
            AddMapping("application/x-msmediaview", ".m13");
            AddMapping("application/x-msmediaview", ".m14");
            AddMapping("video/mpeg", ".m1v");
            AddMapping("audio/x-mpegurl", ".m3u");
            AddMapping("application/x-troff-man", ".man");
            AddMapping("application/x-ms-manifest", ".manifest");
            AddMapping("text/plain", ".map");
            AddMapping("application/x-msaccess", ".mdb");
            AddMapping("application/octet-stream", ".mdp");
            AddMapping("application/x-troff-me", ".me");
            AddMapping("message/rfc822", ".mht");
            AddMapping("message/rfc822", ".mhtml");
            AddMapping("audio/mid", ".mid");
            AddMapping("audio/mid", ".midi");
            AddMapping("application/octet-stream", ".mix");
            AddMapping("application/x-smaf", ".mmf");
            AddMapping("text/xml", ".mno");
            AddMapping("application/x-msmoney", ".mny");
            AddMapping("video/quicktime", ".mov");
            AddMapping("video/x-sgi-movie", ".movie");
            AddMapping("video/mpeg", ".mp2");
            AddMapping("audio/mpeg", ".mp3");
            AddMapping("video/mpeg", ".mpa");
            AddMapping("video/mpeg", ".mpe");
            AddMapping("video/mpeg", ".mpeg");
            AddMapping("video/mpeg", ".mpg");
            AddMapping("application/vnd.ms-project", ".mpp");
            AddMapping("video/mpeg", ".mpv2");
            AddMapping("application/x-troff-ms", ".ms");
            AddMapping("application/octet-stream", ".msi");
            AddMapping("application/octet-stream", ".mso");
            AddMapping("application/x-msmediaview", ".mvb");
            AddMapping("application/x-miva-compiled", ".mvc");
            AddMapping("application/x-netcdf", ".nc");
            AddMapping("video/x-ms-asf", ".nsc");
            AddMapping("message/rfc822", ".nws");
            AddMapping("application/octet-stream", ".ocx");
            AddMapping("application/oda", ".oda");
            AddMapping("text/x-ms-odc", ".odc");
            AddMapping("application/oleobject", ".ods");
            AddMapping("application/onenote", ".one");
            AddMapping("application/onenote", ".onea");
            AddMapping("application/onenote", ".onetoc");
            AddMapping("application/onenote", ".onetoc2");
            AddMapping("application/onenote", ".onetmp");
            AddMapping("application/onenote", ".onepkg");
            AddMapping("application/opensearchdescription+xml", ".osdx");
            AddMapping("application/pkcs10", ".p10");
            AddMapping("application/x-pkcs12", ".p12");
            AddMapping("application/x-pkcs7-certificates", ".p7b");
            AddMapping("application/pkcs7-mime", ".p7c");
            AddMapping("application/pkcs7-mime", ".p7m");
            AddMapping("application/x-pkcs7-certreqresp", ".p7r");
            AddMapping("application/pkcs7-signature", ".p7s");
            AddMapping("image/x-portable-bitmap", ".pbm");
            AddMapping("application/octet-stream", ".pcx");
            AddMapping("application/octet-stream", ".pcz");
            AddMapping("application/pdf", ".pdf");
            AddMapping("application/octet-stream", ".pfb");
            AddMapping("application/octet-stream", ".pfm");
            AddMapping("application/x-pkcs12", ".pfx");
            AddMapping("image/x-portable-graymap", ".pgm");
            AddMapping("application/vnd.ms-pki.pko", ".pko");
            AddMapping("application/x-perfmon", ".pma");
            AddMapping("application/x-perfmon", ".pmc");
            AddMapping("application/x-perfmon", ".pml");
            AddMapping("application/x-perfmon", ".pmr");
            AddMapping("application/x-perfmon", ".pmw");
            AddMapping("image/png", ".png");
            AddMapping("image/x-portable-anymap", ".pnm");
            AddMapping("image/png", ".pnz");
            AddMapping("application/vnd.ms-powerpoint", ".pot");
            AddMapping("application/vnd.ms-powerpoint.template.macroEnabled.12", ".potm");
            AddMapping("application/vnd.openxmlformats-officedocument.presentationml.template", ".potx");
            AddMapping("application/vnd.ms-powerpoint.addin.macroEnabled.12", ".ppam");
            AddMapping("image/x-portable-pixmap", ".ppm");
            AddMapping("application/vnd.ms-powerpoint", ".pps");
            AddMapping("application/vnd.ms-powerpoint.slideshow.macroEnabled.12", ".ppsm");
            AddMapping("application/vnd.openxmlformats-officedocument.presentationml.slideshow", ".ppsx");
            AddMapping("application/vnd.ms-powerpoint", ".ppt");
            AddMapping("application/vnd.ms-powerpoint.presentation.macroEnabled.12", ".pptm");
            AddMapping("application/vnd.openxmlformats-officedocument.presentationml.presentation", ".pptx");
            AddMapping("application/pics-rules", ".prf");
            AddMapping("application/octet-stream", ".prm");
            AddMapping("application/octet-stream", ".prx");
            AddMapping("application/postscript", ".ps");
            AddMapping("application/octet-stream", ".psd");
            AddMapping("application/octet-stream", ".psm");
            AddMapping("application/octet-stream", ".psp");
            AddMapping("application/x-mspublisher", ".pub");
            AddMapping("video/quicktime", ".qt");
            AddMapping("application/x-quicktimeplayer", ".qtl");
            AddMapping("application/octet-stream", ".qxd");
            AddMapping("audio/x-pn-realaudio", ".ra");
            AddMapping("audio/x-pn-realaudio", ".ram");
            AddMapping("application/octet-stream", ".rar");
            AddMapping("image/x-cmu-raster", ".ras");
            AddMapping("image/vnd.rn-realflash", ".rf");
            AddMapping("image/x-rgb", ".rgb");
            AddMapping("application/vnd.rn-realmedia", ".rm");
            AddMapping("audio/mid", ".rmi");
            AddMapping("application/x-troff", ".roff");
            AddMapping("audio/x-pn-realaudio-plugin", ".rpm");
            AddMapping("application/rtf", ".rtf");
            AddMapping("text/richtext", ".rtx");
            AddMapping("application/x-msschedule", ".scd");
            AddMapping("text/scriptlet", ".sct");
            AddMapping("application/octet-stream", ".sea");
            AddMapping("application/set-payment-initiation", ".setpay");
            AddMapping("application/set-registration-initiation", ".setreg");
            AddMapping("text/sgml", ".sgml");
            AddMapping("application/x-sh", ".sh");
            AddMapping("application/x-shar", ".shar");
            AddMapping("application/x-stuffit", ".sit");
            AddMapping("application/vnd.ms-powerpoint.slide.macroEnabled.12", ".sldm");
            AddMapping("application/vnd.openxmlformats-officedocument.presentationml.slide", ".sldx");
            AddMapping("audio/x-smd", ".smd");
            AddMapping("application/octet-stream", ".smi");
            AddMapping("audio/x-smd", ".smx");
            AddMapping("audio/x-smd", ".smz");
            AddMapping("audio/basic", ".snd");
            AddMapping("application/octet-stream", ".snp");
            AddMapping("application/x-pkcs7-certificates", ".spc");
            AddMapping("application/futuresplash", ".spl");
            AddMapping("application/x-wais-source", ".src");
            AddMapping("application/streamingmedia", ".ssm");
            AddMapping("application/vnd.ms-pki.certstore", ".sst");
            AddMapping("application/vnd.ms-pki.stl", ".stl");
            AddMapping("application/x-sv4cpio", ".sv4cpio");
            AddMapping("application/x-sv4crc", ".sv4crc");
            AddMapping("application/x-shockwave-flash", ".swf");
            AddMapping("application/x-troff", ".t");
            AddMapping("application/x-tar", ".tar");
            AddMapping("application/x-tcl", ".tcl");
            AddMapping("application/x-tex", ".tex");
            AddMapping("application/x-texinfo", ".texi");
            AddMapping("application/x-texinfo", ".texinfo");
            AddMapping("application/x-compressed", ".tgz");
            AddMapping("application/vnd.ms-officetheme", ".thmx");
            AddMapping("application/octet-stream", ".thn");
            AddMapping("image/tiff", ".tif");
            AddMapping("image/tiff", ".tiff");
            AddMapping("application/octet-stream", ".toc");
            AddMapping("application/x-troff", ".tr");
            AddMapping("application/x-msterminal", ".trm");
            AddMapping("text/tab-separated-values", ".tsv");
            AddMapping("application/octet-stream", ".ttf");
            AddMapping("text/plain", ".txt");
            AddMapping("application/octet-stream", ".u32");
            AddMapping("text/iuls", ".uls");
            AddMapping("application/x-ustar", ".ustar");
            AddMapping("text/vbscript", ".vbs");
            AddMapping("text/x-vcard", ".vcf");
            AddMapping("text/plain", ".vcs");
            AddMapping("application/vnd.ms-visio.viewer", ".vdx");
            AddMapping("text/xml", ".vml");
            AddMapping("application/vnd.visio", ".vsd");
            AddMapping("application/vnd.visio", ".vss");
            AddMapping("application/vnd.visio", ".vst");
            AddMapping("application/x-ms-vsto", ".vsto");
            AddMapping("application/vnd.visio", ".vsw");
            AddMapping("application/vnd.visio", ".vsx");
            AddMapping("application/vnd.visio", ".vtx");
            AddMapping("audio/wav", ".wav");
            AddMapping("audio/x-ms-wax", ".wax");
            AddMapping("image/vnd.wap.wbmp", ".wbmp");
            AddMapping("application/vnd.ms-works", ".wcm");
            AddMapping("application/vnd.ms-works", ".wdb");
            AddMapping("application/vnd.ms-works", ".wks");
            AddMapping("video/x-ms-wm", ".wm");
            AddMapping("audio/x-ms-wma", ".wma");
            AddMapping("application/x-ms-wmd", ".wmd");
            AddMapping("application/x-msmetafile", ".wmf");
            AddMapping("text/vnd.wap.wml", ".wml");
            AddMapping("application/vnd.wap.wmlc", ".wmlc");
            AddMapping("text/vnd.wap.wmlscript", ".wmls");
            AddMapping("application/vnd.wap.wmlscriptc", ".wmlsc");
            AddMapping("video/x-ms-wmp", ".wmp");
            AddMapping("video/x-ms-wmv", ".wmv");
            AddMapping("video/x-ms-wmx", ".wmx");
            AddMapping("application/x-ms-wmz", ".wmz");
            AddMapping("application/vnd.ms-works", ".wps");
            AddMapping("application/x-mswrite", ".wri");
            AddMapping("x-world/x-vrml", ".wrl");
            AddMapping("x-world/x-vrml", ".wrz");
            AddMapping("text/xml", ".wsdl");
            AddMapping("video/x-ms-wvx", ".wvx");
            AddMapping("application/directx", ".x");
            AddMapping("x-world/x-vrml", ".xaf");
            AddMapping("application/xaml+xml", ".xaml");
            AddMapping("application/x-silverlight-app", ".xap");
            AddMapping("application/x-ms-xbap", ".xbap");
            AddMapping("image/x-xbitmap", ".xbm");
            AddMapping("text/plain", ".xdr");
            AddMapping("application/vnd.ms-excel", ".xla");
            AddMapping("application/vnd.ms-excel.addin.macroEnabled.12", ".xlam");
            AddMapping("application/vnd.ms-excel", ".xlc");
            AddMapping("application/vnd.ms-excel", ".xlm");
            AddMapping("application/vnd.ms-excel", ".xls");
            AddMapping("application/vnd.ms-excel.sheet.binary.macroEnabled.12", ".xlsb");
            AddMapping("application/vnd.ms-excel.sheet.macroEnabled.12", ".xlsm");
            AddMapping("application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", ".xlsx");
            AddMapping("application/vnd.ms-excel", ".xlt");
            AddMapping("application/vnd.ms-excel.template.macroEnabled.12", ".xltm");
            AddMapping("application/vnd.openxmlformats-officedocument.spreadsheetml.template", ".xltx");
            AddMapping("application/vnd.ms-excel", ".xlw");
            AddMapping("text/xml", ".xml");
            AddMapping("x-world/x-vrml", ".xof");
            AddMapping("image/x-xpixmap", ".xpm");
            AddMapping("application/vnd.ms-xpsdocument", ".xps");
            AddMapping("text/xml", ".xsd");
            AddMapping("text/xml", ".xsf");
            AddMapping("text/xml", ".xsl");
            AddMapping("text/xml", ".xslt");
            AddMapping("application/octet-stream", ".xsn");
            AddMapping("application/octet-stream", ".xtp");
            AddMapping("image/x-xwindowdump", ".xwd");
            AddMapping("application/x-compress", ".z");
            AddMapping("application/x-zip-compressed", ".zip");
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
