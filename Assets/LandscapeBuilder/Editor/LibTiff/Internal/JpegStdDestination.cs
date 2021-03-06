using BitMiracle.LibJpeg.Classic;

namespace BitMiracle.LibTiff.Classic.Internal
{
    /// <summary>
    /// JPEG library destination data manager.
    /// These routines direct compressed data from LibJpeg.Net into the
    /// LibTiff.Net output buffer.
    /// </summary>
    class JpegStdDestination : jpeg_destination_mgr
    {
        private Tiff m_tif;

        public JpegStdDestination(Tiff tif)
        {
            m_tif = tif;
        }

        public override void init_destination()
        {
            initInternalBuffer(m_tif.m_rawdata, 0);
        }

        public override bool empty_output_buffer()
        {
            /* the entire buffer has been filled */
            m_tif.m_rawcc = m_tif.m_rawdatasize;
            m_tif.flushData1();

            initInternalBuffer(m_tif.m_rawdata, 0);
            return true;
        }

        public override void term_destination()
        {
            m_tif.m_rawcp = m_tif.m_rawdatasize - freeInBuffer;
            m_tif.m_rawcc = m_tif.m_rawdatasize - freeInBuffer;
            /* NB: LibTiff.Net does the final buffer flush */
        }
    }
}
