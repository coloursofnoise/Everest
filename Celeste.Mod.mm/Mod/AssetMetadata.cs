﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Mono.Cecil;
using MonoMod;
using MonoMod.Helpers;
using MonoMod.InlineRT;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Celeste.Mod {
    public class AssetMetadata {

        public SourceType Source;
        public Type AssetType = null;
        public string AssetFormat = null;

        public bool HasData = true;

        public string PathRelative;
        public string PathSource;
        public string PathArchive;

        public Assembly Assembly;
        public string AssemblyName;

        public long Offset;
        public int Length;

        public List<AssetMetadata> Children = new List<AssetMetadata>();

        /// <summary>
        /// Returns a new stream to read the data from.
        /// In case of limited data (Length is set), LimitedStream is used.
        /// </summary>
        public Stream Stream {
            get {
                if (!HasData) return null;
                Stream stream = null;
                if (Source == SourceType.Filesystem) {
                    stream = File.OpenRead(PathSource);
                } else if (Source == SourceType.Zip) {
                    string file = PathSource.Replace('\\', '/');
                    using (Stream zipStream = File.OpenRead(PathArchive))
                    using (ZipArchive zip = new ZipArchive(zipStream, ZipArchiveMode.Read)) {
                            foreach (ZipArchiveEntry entry in zip.Entries) {
                            if (entry.FullName.Replace('\\', '/') == file) {
                                MemoryStream ms = new MemoryStream();
                                using (Stream entryStream = entry.Open())
                                    entryStream.CopyTo(ms);
                                ms.Seek(0, SeekOrigin.Begin);
                                stream = ms;
                            }
                        }
                    }
                } else if (Source == SourceType.Assembly) {
                    stream = Assembly.GetManifestResourceStream(PathSource);
                }

                if (stream == null || Length == 0) {
                    return stream;
                }
                return new LimitedStream(stream, Offset, Length);
            }
        }

        /// <summary>
        /// Returns the files contents.
        /// </summary>
        public byte[] Data {
            get {
                if (!HasData) return null;
                using (Stream stream = Stream) {
                    if (stream is MemoryStream) {
                        return ((MemoryStream) stream).GetBuffer();
                    }

                    using (MemoryStream ms = new MemoryStream()) {
                        byte[] buffer = new byte[2048];
                        int read;
                        while (0 < (read = stream.Read(buffer, 0, buffer.Length))) {
                            ms.Write(buffer, 0, read);
                        }
                        return ms.ToArray();
                    }
                }
            }
        }

        public AssetMetadata() {
            Source = SourceType.Meta;
        }

        public AssetMetadata(string file)
            : this(file, 0, 0) {
        }
        public AssetMetadata(string file, long offset, int length)
            : this() {
            Source = SourceType.Filesystem;
            PathSource = file;
            Offset = offset;
            Length = length;
        }

        public AssetMetadata(string zip, string file)
            : this(file) {
            Source = SourceType.Zip;
            PathArchive = zip;
            PathSource = file;
        }

        public AssetMetadata(Assembly assembly, string file)
            : this(file) {
            Source = SourceType.Assembly;
            Assembly = assembly;
            AssemblyName = assembly.GetName().Name;
        }

        public enum SourceType {
            Meta,
            Filesystem,
            Zip,
            Assembly,
        }

    }
}
