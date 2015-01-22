using System;
using System.IO;
using Lucene.Net.Analysis.Standard;
using Lucene.Net.Index;
using Lucene.Net.Store;

namespace NHibernate.Indexer.Engine
{
    class FSDirectoryProvider : IDisposable
    {
        private FSDirectory directory;
        private bool isUpdate;
        private IndexWriter indexWriter;

        public FSDirectory Directory
        {
            get { return directory; }
        }

        public bool IsUpdate
        {
            get { return isUpdate; }
        }

        public IndexWriter GetIndexWriter
        {
            get { return indexWriter; }
        }

        public FSDirectoryProvider Initialize(string indexDir)
        {
            try
            {
                directory = FSDirectory.Open(new DirectoryInfo(indexDir), new NativeFSLockFactory());
                isUpdate = IndexReader.IndexExists(directory);

                if (isUpdate)
                {
                    if (IndexWriter.IsLocked(directory))
                    {
                        IndexWriter.Unlock(directory);
                    }
                }

                indexWriter = new IndexWriter(directory, new StandardAnalyzer(Lucene.Net.Util.Version.LUCENE_30), !isUpdate, IndexWriter.MaxFieldLength.UNLIMITED);
            }
            catch (IOException e)
            {
                throw new HibernateException("Unable to initialize index: " + indexDir, e);
            }
            return this;
        }

        public void Dispose()
        {
            if (indexWriter != null)
            {
                indexWriter.Optimize();
                indexWriter.Commit();
                indexWriter.Dispose();
            }

            if (directory != null)
                directory.Dispose();
        }
    }
}
