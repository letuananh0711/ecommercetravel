// ********************************************************************
// Document     : Pagination.cs
// Version      : 1.0
// Category     : Libraries
// Description  : This is a pagination class for creating different types 
//                of page links.
// Author       : Mohammad Jahedur Rahman
// Date         : 08/03/2008
// Copyright    : Copyright (c) Valentica. All rights reserved.
// ********************************************************************
// Edited: CUONG VU
using System;
using System.IO;
using System.Text;
using System.Xml.Linq;

namespace Libraries
{
    /// <summary>
    /// Pagination type enumeration
    /// </summary>
    public enum PaginationType
    {
        /// <summary>
        /// Default page link
        /// </summary>
        Default,

        /// <summary>
        /// Default page link with item on the right 
        /// </summary>
        DefaultWithItemRight,

        /// <summary>
        /// Default page link with item on the left
        /// </summary>
        DefaultWithItemLeft,

        /// <summary>
        /// Previous, next page link
        /// </summary>
        PreviousNext,

        /// <summary>
        /// Previous, next page link with item on the right
        /// </summary>
        PreviousNextItemRight,

        /// <summary>
        /// Previous, next page link with item on the left
        /// </summary>
        PreviousNextItemLeft,

        /// <summary>
        /// Previous, next page link with item on the center
        /// </summary>
        PreviousNextItemCenter,

        /// <summary>
        /// First, previous, next, last page link
        /// </summary>
        FirstPreviousNextLast,

        /// <summary>
        /// First, previous, next, last page link with item on the right
        /// </summary>
        FirstPreviousNextLastItemRight,

        /// <summary>
        /// First, previous, next, last page link with item on the left
        /// </summary>
        FirstPreviousNextLastItemLeft,

        /// <summary>
        /// First, previous, next, last page link with item on the center
        /// </summary>
        FirstPreviousNextLastItemCenter,

        /// <summary>
        /// Number page link
        /// </summary>
        Number,

        /// <summary>
        /// Number page link with item on the right
        /// </summary>
        NumberWithItemRight,

        /// <summary>
        /// Number page link with item on the left
        /// </summary>
        NumberWithItemLeft
    }

    /// <summary>
    /// Item type enumeration
    /// </summary>
    public enum ItemTypes
    {
        /// <summary>
        /// Displaying (n) to (n) of (n) Items
        /// </summary>
        Item,

        /// <summary>
        /// Page (n) of (n)
        /// </summary>
        Page,

        /// <summary>
        /// (n)/(n)
        /// </summary>
        Digit
    }

    /// <summary>
    /// Pagination class
    /// </summary>
    public class Pagination
    {
        #region Private Properties

        #region General Properties

        private int _perPage = 10;   // Max number of items you want shown per page
        private int _numLinks = 2;    // Number of "digit" links to show before/after the currently viewed page

        #endregion

        #region Link Properties

        private string _firstLink = "&lsaquo; First";
        private string _nextLink = "&gt;";
        private string _prevLink = "&lt;";
        private string _lastLink = "Last &rsaquo;";

        #endregion

        #region Tag Properties

        private string _fullTagOpen = "";
        private string _fullTagClose = "";
        private string _firstTagOpen = "";
        private string _firstTagClose = "&nbsp;";
        private string _lastTagOpen = "&nbsp;";
        private string _lastTagClose = "";
        private string _curTagOpen = "&nbsp;";
        private string _curTagClose = "";
        private string _nextTagOpen = "&nbsp;";
        private string _nextTagClose = "&nbsp;";
        private string _prevTagOpen = "&nbsp;";
        private string _prevTagClose = "";
        private string _numTagOpen = "&nbsp;";
        private string _numTagClose = "";
        private string _itemTagOpen = "<strong>&nbsp;&nbsp;";
        private string _itemTagClose = "</strong>&nbsp;&nbsp;";

        #endregion

        private ItemTypes _itemType = ItemTypes.Item;

        #endregion

        #region Public Property Accessors

        #region General Property Accessors

        /// <summary>
        /// Gets or sets url to controller/action.
        /// </summary>
        public string BaseUrl { get; set; }

        /// <summary>
        /// Gets or sets the total rows in the result set.
        /// </summary>
        public int TotalRows { get; set; }

        /// <summary>
        /// Gets or sets the number item per page.
        /// </summary>
        public int PerPage
        {
            get { return _perPage; }
            set { _perPage = value; }
        }

        /// <summary>
        /// Gets or sets the number of "digit" links before and after the selected page number.
        /// </summary>
        public int NumLinks
        {
            get { return _numLinks; }
            set { _numLinks = value; }
        }

        /// <summary>
        /// Gets or sets the current selected page.
        /// </summary>
        public int CurPage { get; set; }

        #endregion

        #region Link Property Accessors

        /// <summary>
        /// Gets or sets the text to be shown in the "first" link on the left.
        /// </summary>
        public string FirstLink
        {
            get { return _firstLink; }
            set { _firstLink = value; }
        }

        /// <summary>
        /// Gets or sets the text to be shown in the "next" page link.
        /// </summary>
        public string NextLink
        {
            get { return _nextLink; }
            set { _nextLink = value; }
        }

        /// <summary>
        /// Gets or sets the text to be shown in the "previous" page link.
        /// </summary>
        public string PrevLink
        {
            get { return _prevLink; }
            set { _prevLink = value; }
        }

        /// <summary>
        /// Gets or sets the text to be shown in the "last" link on the right.
        /// </summary>
        public string LastLink
        {
            get { return _lastLink; }
            set { _lastLink = value; }
        }

        #endregion

        #region Tag Property Accessors

        /// <summary>
        /// Gets or sets the opening tag placed on the left side of the entire result.
        /// </summary>
        public string FullTagOpen
        {
            get { return _fullTagOpen; }
            set { _fullTagOpen = value; }
        }

        /// <summary>
        /// Gets or sets the closing tag placed on the right side of the entire result.
        /// </summary>
        public string FullTagClose
        {
            get { return _fullTagClose; }
            set { _fullTagClose = value; }
        }

        /// <summary>
        /// Gets or sets the opening tag for the "first" link.
        /// </summary>
        public string FirstTagOpen
        {
            get { return _firstTagOpen; }
            set { _firstTagOpen = value; }
        }

        /// <summary>
        /// Gets or sets the closing tag for the "first" link.
        /// </summary>
        public string FirstTagClose
        {
            get { return _firstTagClose; }
            set { _firstTagClose = value; }
        }

        /// <summary>
        /// Gets or sets the opening tag for the "last" link.
        /// </summary>
        public string LastTagOpen
        {
            get { return _lastTagOpen; }
            set { _lastTagOpen = value; }
        }

        /// <summary>
        /// Gets or sets the closing tag for the "last" link.
        /// </summary>
        public string LastTagClose
        {
            get { return _lastTagClose; }
            set { _lastTagClose = value; }
        }

        /// <summary>
        /// Gets or sets the opening tag for the "current" link.
        /// </summary>
        public string CurTagOpen
        {
            get { return _curTagOpen; }
            set { _curTagOpen = value; }
        }

        /// <summary>
        /// Gets or sets the closing tag for the "current" link.
        /// </summary>
        public string CurTagClose
        {
            get { return _curTagClose; }
            set { _curTagClose = value; }
        }

        /// <summary>
        /// Gets or sets the opening tag for the "next" link.
        /// </summary>
        public string NextTagOpen
        {
            get { return _nextTagOpen; }
            set { _nextTagOpen = value; }
        }

        /// <summary>
        /// Gets or sets the closing tag for the "next" link.
        /// </summary>
        public string NextTagClose
        {
            get { return _nextTagClose; }
            set { _nextTagClose = value; }
        }

        /// <summary>
        /// Gets or sets the opening tag for the "previous" link.
        /// </summary>
        public string PrevTagOpen
        {
            get { return _prevTagOpen; }
            set { _prevTagOpen = value; }
        }

        /// <summary>
        /// Gets or sets the closing tag for the "previous" link.
        /// </summary>
        public string PrevTagClose
        {
            get { return _prevTagClose; }
            set { _prevTagClose = value; }
        }

        /// <summary>
        /// Gets or sets the opening tag for the "digit" link.
        /// </summary>
        public string NumTagOpen
        {
            get { return _numTagOpen; }
            set { _numTagOpen = value; }
        }

        /// <summary>
        /// Gets or sets the closing tag for the "digit" link.
        /// </summary>
        public string NumTagClose
        {
            get { return _numTagClose; }
            set { _numTagClose = value; }
        }

        /// <summary>
        /// Gets or sets the opening tag for the "Item".
        /// </summary>
        public string ItemTagOpen
        {
            get { return _itemTagOpen; }
            set { _itemTagOpen = value; }
        }

        /// <summary>
        /// Gets or sets the closing tag for the "Item".
        /// </summary>
        public string ItemTagClose
        {
            get { return _itemTagClose; }
            set { _itemTagClose = value; }
        }

        #endregion

        /// <summary>
        /// Gets or sets the item type.
        /// </summary>
        public ItemTypes ItemType
        {
            get { return _itemType; }
            set { _itemType = value; }
        }

        #endregion

        /// <summary>
        /// Gets or sets the total number of pages
        /// </summary>
        private int NumPages { get; set; }

        #region Public Constructors
        // EDITED BY CUONG VU - 12/11/2012
        public Pagination(string baseURL, string URL_segment, int totalRows)
        {
            string pageString = "";
            pageString = URL_segment;
            int cur_page = (String.IsNullOrEmpty(pageString)) ? 1 : Int32.Parse(pageString);
            int per_page = 10;
            BaseUrl = baseURL;
            TotalRows = totalRows;
            CurPage = cur_page;
            PerPage = per_page;
            PrevLink = "Prev";
            NextLink = "Next";
        }

        public Pagination(bool loadFromXml)
        {
            if (!loadFromXml)
            {
                return;
            }

            string file = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Pagination.xml");

            this.LoadPaginationXml(file);
        }

        /// <summary>
        /// Load the Pagination Configuration from Xml
        /// </summary>
        /// <param name="xmlFile">string, Pagination.xml File Path</param>
        public void LoadPaginationXml(string xmlFile)
        {
            XElement element;

            try
            {
                element = XElement.Load(xmlFile);

                var properties = element.Elements();

                foreach (var property in properties)
                {
                    SetPaginationSettings(property.Name.ToString(), property.Value);
                }
            }
            catch (FileNotFoundException)
            {

            }
            catch (Exception)
            {

            }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Get the page links
        /// </summary>
        /// <returns>Page link string</returns>
        public string GetPageLinks()
        {
            return this.GetPageLinks(PaginationType.Default);
        }
      
        /// <summary>
        /// Get the page links
        /// </summary>
        /// <param name="pageType">Pagination type</param>
        /// <returns>Page link string</returns>
        public string GetPageLinks(PaginationType pageType)
        {
            // If anything invalid
            if (!this.IsValidSettings())
                return "";

            // Add a trailing slash to the base URL if needed
            if (!this.BaseUrl.Trim().EndsWith("/"))
                this.BaseUrl = this.BaseUrl + "/";

            // And here we go...
            StringBuilder pageLink = new StringBuilder();

            switch (pageType)
            {
                case PaginationType.Default:
                    this.BuildDefaultLink(pageLink);
                    break;

                case PaginationType.DefaultWithItemRight:
                    this.BuildDefaultLink(pageLink);
                    this.BuildItemLink(pageLink);
                    break;

                case PaginationType.DefaultWithItemLeft:
                    this.BuildItemLink(pageLink);
                    this.BuildDefaultLink(pageLink);
                    break;

                case PaginationType.PreviousNext:
                    this.BuildPreviousNextLink(pageLink);
                    break;

                case PaginationType.PreviousNextItemRight:
                    this.BuildPreviousNextLink(pageLink);
                    this.BuildItemLink(pageLink);
                    break;

                case PaginationType.PreviousNextItemLeft:
                    this.BuildItemLink(pageLink);
                    this.BuildPreviousNextLink(pageLink);
                    break;

                case PaginationType.PreviousNextItemCenter:
                    this.BuildPreviousLink(pageLink, false);
                    this.BuildItemLink(pageLink);
                    this.BuildNextLink(pageLink, false);
                    break;

                case PaginationType.FirstPreviousNextLast:
                    this.BuildFirstPreviousNextLastLink(pageLink);
                    break;

                case PaginationType.FirstPreviousNextLastItemRight:
                    this.BuildFirstPreviousNextLastLink(pageLink);
                    this.BuildItemLink(pageLink);
                    break;

                case PaginationType.FirstPreviousNextLastItemLeft:
                    this.BuildItemLink(pageLink);
                    this.BuildFirstPreviousNextLastLink(pageLink);
                    break;

                case PaginationType.FirstPreviousNextLastItemCenter:
                    this.BuildFirstLink(pageLink, false);
                    this.BuildPreviousLink(pageLink, false);
                    this.BuildItemLink(pageLink);
                    this.BuildNextLink(pageLink, false);
                    this.BuildLastLink(pageLink, false);
                    break;

                case PaginationType.Number:
                    this.BuildDigitLink(pageLink);
                    break;

                case PaginationType.NumberWithItemRight:
                    this.BuildDigitLink(pageLink);
                    this.BuildItemLink(pageLink);
                    break;

                case PaginationType.NumberWithItemLeft:
                    this.BuildItemLink(pageLink);
                    this.BuildDigitLink(pageLink);
                    break;

                default:
                    this.GetPageLinks(PaginationType.Default);
                    break;
            }

            pageLink.Insert(0, this.FullTagOpen);
            pageLink.Append(this.FullTagClose);

            return pageLink.ToString();
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Set the Pagination Configuration Settings
        /// </summary>
        /// <param name="propertyName">Configuration Property Name</param>
        /// <param name="propertyValue">Configuration Property Value</param>
        private void SetPaginationSettings(string propertyName, string propertyValue)
        {
            switch (propertyName)
            {
                #region Set General Properties

                case "BaseUrl":
                    this.BaseUrl = propertyValue;
                    break;

                case "TotalRows":
                    this.TotalRows = Convert.ToInt32(propertyValue);
                    break;

                case "PerPage":
                    this.PerPage = Convert.ToInt32(propertyValue);
                    break;

                case "NumLinks":
                    this.NumLinks = Convert.ToInt32(propertyValue);
                    break;

                case "CurPage":
                    this.CurPage = Convert.ToInt32(propertyValue);
                    break;

                #endregion

                #region Set Link Properties

                case "FirstLink":
                    this.FirstLink = propertyValue;
                    break;

                case "NextLink":
                    this.NextLink = propertyValue;
                    break;

                case "PrevLink":
                    this.PrevLink = propertyValue;
                    break;

                case "LastLink":
                    this.LastLink = propertyValue;
                    break;

                #endregion

                #region Set Tag Properties

                case "FullTagOpen":
                    this.FullTagOpen = propertyValue;
                    break;

                case "FullTagClose":
                    this.FullTagClose = propertyValue;
                    break;

                case "FirstTagOpen":
                    this.FirstTagOpen = propertyValue;
                    break;

                case "FirstTagClose":
                    this.FirstTagClose = propertyValue;
                    break;

                case "LastTagOpen":
                    this.LastTagOpen = propertyValue;
                    break;

                case "LastTagClose":
                    this.LastTagClose = propertyValue;
                    break;

                case "CurTagOpen":
                    this.CurTagOpen = propertyValue;
                    break;

                case "CurTagClose":
                    this.CurTagClose = propertyValue;
                    break;

                case "NextTagOpen":
                    this.NextTagOpen = propertyValue;
                    break;

                case "NextTagClose":
                    this.NextTagClose = propertyValue;
                    break;

                case "PrevTagOpen":
                    this.PrevTagOpen = propertyValue;
                    break;

                case "PrevTagClose":
                    this.PrevTagClose = propertyValue;
                    break;

                case "NumTagOpen":
                    this.NumTagOpen = propertyValue;
                    break;

                case "NumTagClose":
                    this.NumTagClose = propertyValue;
                    break;

                #endregion

                default:
                    break;
            }
        }

        /// <summary>
        /// Check if everythings are ok
        /// </summary>
        /// <returns>Boolean</returns>
        private bool IsValidSettings()
        {
            // If our item count or per page total is zero there is no need to continue.
            if (this.TotalRows == 0 && this.PerPage == 0)
                return false;

            // Calculate the total number of pages
            this.NumPages = Convert.ToInt32(Math.Ceiling(Convert.ToDouble(this.TotalRows) / Convert.ToDouble(this.PerPage)));

            // Is there only one page? Hm... nothing more to do here then.
            if (NumPages == 1)
                return false;

            // Is the current page equals to 0 then return
            if (this.CurPage == 0)
                return false;

            // Is the current page beyond the result range then return
            if (((this.CurPage - 1) * this.PerPage) > this.TotalRows)
                return false;

            return true;
        }

        /// <summary>
        /// Build the "First" link
        /// </summary>
        /// <param name="pageLink">StringBuilder, pageLink</param>
        /// <param name="isDefault">bool, isDefault</param>
        // EDITED BY CUONG VU - 12/11/2012
        private void BuildFirstLink(StringBuilder pageLink, bool isDefault)
        {
            if (isDefault)
            {
                if (this.CurPage > this.NumLinks)
                {
                    pageLink.Append(this.FirstTagOpen);
                    pageLink.Append("<li><a class='paginate button-link gray' href='" + this.BaseUrl + "1" + "'>");
                    pageLink.Append(this.FirstLink);
                    pageLink.Append("</a></li>");
                    pageLink.Append(this.FirstTagClose);
                }
            }
            else
            {
                pageLink.Append(this.FirstTagOpen);

                if (this.CurPage != 1)
                {
                    pageLink.Append("<li><a class='paginate button-link gray' href='" + this.BaseUrl + "1" + "'>");
                    pageLink.Append(this.FirstLink);
                    pageLink.Append("</a></li>");
                }
                else
                {
                    pageLink.Append(this.FirstLink);
                }

                pageLink.Append(this.FirstTagClose);
            }
        }

        /// <summary>
        /// Build the "Previous" link
        /// </summary>
        /// <param name="pageLink">StringBuilder, pageLink</param>
        /// <param name="isDefault">bool, isDefault</param>
        /// EDITED BY CUONG VU - 12/11/2012
        private void BuildPreviousLink(StringBuilder pageLink, bool isDefault)
        {
            if (isDefault)
            {
                if ((this.CurPage - this.NumLinks) >= 0)
                {
                    int i = this.CurPage - 1;

                    if (i == 0) i = 1;

                    pageLink.Append(this.PrevTagOpen);
                    pageLink.Append("<li><a class='paginate button-link gray disabled' href='" + this.BaseUrl + Convert.ToString(i) + "'>");
                    pageLink.Append(this.PrevLink);
                    pageLink.Append("</a></li>");
                    pageLink.Append(this.PrevTagClose);
                }
            }
            else
            {
                int i = this.CurPage - 1;

                pageLink.Append(this.PrevTagOpen);

                if (i > 0)
                {
                    pageLink.Append("<li><a class='paginate button-link gray' href='" + this.BaseUrl + Convert.ToString(i) + "'>");
                    pageLink.Append(this.PrevLink);
                    pageLink.Append("</a></li>");
                }
                else
                {
                    pageLink.Append(this.PrevLink);
                }

                pageLink.Append(this.PrevTagClose);
            }
        }

        /// <summary>
        /// Build the "Digits" link
        /// </summary>
        /// <param name="pageLink">StringBuilder, pageLink</param>
        /// EDITED BY CUONG VU - 12/11/2012
        private void BuildDigitLink(StringBuilder pageLink)
        {
            // Calculate the start and end numbers. These determine
            // which number to start and end the digit links with
            int start = ((this.CurPage - this.NumLinks) > 0) ? this.CurPage - (this.NumLinks - 1) : 1;
            int end = ((this.CurPage + this.NumLinks) < NumPages) ? this.CurPage + this.NumLinks : NumPages;

            for (int loop = start - 1; loop <= end; loop++)
            {
                int i = (loop * this.PerPage) - this.PerPage;

                if (i >= 0)
                {
                    // Current page
                    if (this.CurPage == loop)
                    {
                        pageLink.Append(this.CurTagOpen);
                        pageLink.Append("<li class='active'><a class='button-link active_page' href='#'>" + loop + "</a></li>");
                        pageLink.Append(this.CurTagClose);
                    }
                    else
                    {
                        pageLink.Append(this.NumTagOpen);
                        pageLink.Append("<li><a class='paginate button-link gray' href='" + this.BaseUrl + Convert.ToString(loop) + "'>");
                        pageLink.Append(loop);
                        pageLink.Append("</a></li>");
                        pageLink.Append(this.NumTagClose);
                    }
                }
            }
        }

        /// <summary>
        /// Build the "Next" Link
        /// </summary>
        /// <param name="pageLink">StringBuilder, pageLink</param>
        /// <param name="isDefault">bool, isDefault</param>
        /// EDITED BY CUONG VU - 12/11/2012
        private void BuildNextLink(StringBuilder pageLink, bool isDefault)
        {
            if (isDefault)
            {
                if (this.CurPage < NumPages)
                {
                    pageLink.Append(this.NextTagOpen);
                    pageLink.Append("<li><a href='" + this.BaseUrl + Convert.ToString(this.CurPage + 1) + "'>");
                    pageLink.Append(this.NextLink);
                    pageLink.Append("</a></li>");
                    pageLink.Append(this.NextTagClose);
                }
            }
            else
            {
                int i = this.CurPage + 1;

                pageLink.Append(this.NextTagOpen);

                if (i <= NumPages)
                {
                    pageLink.Append("<a href='" + this.BaseUrl + Convert.ToString(i) + "'>");
                    pageLink.Append(this.NextLink);
                    pageLink.Append("</a>");
                }
                else
                {
                    pageLink.Append(this.NextLink);
                }

                pageLink.Append(this.NextTagClose);
            }
        }

        /// <summary>
        /// Build the "Last" link
        /// </summary>
        /// <param name="pageLink">StringBuilder, pageLink</param>
        /// <param name="isDefault">bool, isDefault</param>
        /// EDITED BY CUONG VU - 12/11/2012
        private void BuildLastLink(StringBuilder pageLink, bool isDefault)
        {
            if (isDefault)
            {
                if ((this.CurPage + this.NumLinks) < NumPages)
                {
                    pageLink.Append(this.LastTagOpen);
                    pageLink.Append("<li><a class='paginate button-link gray' href='" + this.BaseUrl + Convert.ToString(NumPages) + "'>");
                    pageLink.Append(this.LastLink);
                    pageLink.Append("</a></li>");
                    pageLink.Append(this.LastTagClose);
                }
            }
            else
            {
                pageLink.Append(this.LastTagOpen);

                if (this.CurPage != NumPages)
                {
                    pageLink.Append("<li><a class='paginate button-link gray' href='" + this.BaseUrl + Convert.ToString(NumPages) + "'>");
                    pageLink.Append(this.LastLink);
                    pageLink.Append("</a></li>");
                }
                else
                {
                    pageLink.Append(this.LastLink);
                }

                pageLink.Append(this.LastTagClose);
            }
        }

        /// <summary>
        /// Build the "Item" link
        /// </summary>
        /// <param name="pageLink">StringBuilder, pageLink</param>
        private void BuildItemLink(StringBuilder pageLink)
        {
            pageLink.Append(this.ItemTagOpen);

            if (this.ItemType == ItemTypes.Item)
            {
                int start = (this.CurPage - 1) * this.PerPage + 1;
                int end = this.CurPage * this.PerPage;

                end = (end > this.TotalRows) ? this.TotalRows : end;

                pageLink.Append("Displaying " + start.ToString() +
                                " to " + end.ToString() +
                                " of " + this.TotalRows.ToString() + " Items");
            }
            else if (this.ItemType == ItemTypes.Page)
                pageLink.Append("Page " + this.CurPage.ToString() + " of " + this.NumPages.ToString());
            else if (this.ItemType == ItemTypes.Digit)
                pageLink.Append(this.CurPage.ToString() + "/" + this.NumPages.ToString());

            pageLink.Append(this.ItemTagClose);
        }

        /// <summary>
        /// Build the Default link
        /// </summary>
        /// <param name="pageLink">StringBuilder, pageLink</param>
        private void BuildDefaultLink(StringBuilder pageLink)
        {
            this.BuildFirstLink(pageLink, true);
            this.BuildPreviousLink(pageLink, true);
            this.BuildDigitLink(pageLink);
            this.BuildNextLink(pageLink, true);
            this.BuildLastLink(pageLink, true);
        }

        /// <summary>
        /// Build the PreviousNext link
        /// </summary>
        /// <param name="pageLink">StringBuilder, pageLink</param>
        private void BuildPreviousNextLink(StringBuilder pageLink)
        {
            this.BuildPreviousLink(pageLink, false);
            this.BuildNextLink(pageLink, false);
        }

        /// <summary>
        /// Build the FirstPreviousNextLast link
        /// </summary>
        /// <param name="pageLink">StringBuilder, pageLink</param>
        private void BuildFirstPreviousNextLastLink(StringBuilder pageLink)
        {
            this.BuildFirstLink(pageLink, false);
            this.BuildPreviousLink(pageLink, false);
            this.BuildNextLink(pageLink, false);
            this.BuildLastLink(pageLink, false);
        }
        #endregion
    }
}