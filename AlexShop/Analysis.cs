using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlexShop
{
    class Analysis
    {

        //Pages list.
        public List<List<AnalysisItem>> m_pages;

        //Current page index.
        private int currPage = 0;

        //Constructor.
        public Analysis()
        {
            m_pages = new List<List<AnalysisItem>>();
        }


        //Get Right End.
        public bool GetRightEnd()
        {
            return currPage >= m_pages.Count;
        }


        //Get next page.
        public List<AnalysisItem> NextPage()
        {

            //If the pages are empty, return an empty page.
            if (m_pages.Count == 0)
                return new List<AnalysisItem>();

            //If current page is not out of index.
            if (currPage < m_pages.Count)
            {
                currPage++;
                return m_pages.ElementAt(currPage - 1);
            }

            //Else return an empty list.
            return m_pages.ElementAt(currPage - 1);
        }




        //Get Previous Page.
        public List<AnalysisItem> PrevPage()
        {
            //If the pages are empty, return an empty page.
            if (m_pages.Count == 0)
                return new List<AnalysisItem>();


            //Return the prev page.
            if (currPage - 2 >= 0)
            {
                currPage--;
                return m_pages.ElementAt(currPage - 1);
            }

            //No other previous page, this is the first page!!!.
            return m_pages.ElementAt(0);
        }

    }
}
