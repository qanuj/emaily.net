using System;
using System.Collections.Generic;
using Emaily.Core.Enumerations;

namespace Emaily.Core.DTO
{
    public class CampaignVM
    {
        public int Id { get; set; }
        public string Errors { get; set; }
        public string Title { get; set; }
        public int? Recipients { get; set; }
        public DateTime? Started { get; set; }
        public int? UniqueOpens { get; set; }
        public int? UniqueClicks { get; set; }
        public CampaignStatusEnum Status { get; set; }
    }

    public class CampaignReportVM
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public int? Recipients { get; set; }
        public DateTime Started { get; set; }
        public int? UniqueOpens { get; set; }
        public int? UniqueClicks { get; set; }
        public CampaignStatusEnum Status { get; set; }
    }

    public class TemplateVM
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public DateTime? Created { get; set; }
    }

    public class TemplateInfoVM
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime? Created { get; set; }
        public string Custom { get; set; }
        public IEnumerable<int> Lists { get; set; }
        public string PlainText { get; set; }
        public string HtmlText { get; set; }
        public string QueryString { get; set; }
        public bool IsHtml { get; set; }
        public int AppId { get; set; }
        public SenderViewModel Sender { get; set; }
    }

    public class CampaignInfoVM
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Label { get; set; }
        public string Custom { get; set; }
        public IEnumerable<int> Lists { get; set; }
        public DateTime? Future { get; set; }
        public string PlainText { get; set; }
        public string HtmlText { get; set; }
        public string QueryString { get; set; }
        public bool IsHtml { get; set; }
        public SenderViewModel Sender { get; set; }
        public int AppId { get; set; }
    }

    public class SenderViewModel
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string ReplyTo { get; set; }
    }
}