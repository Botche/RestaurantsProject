﻿namespace RestaurantsPlatform.Web.Areas.Restaurants.Controllers
{
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Security.Claims;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using RestaurantsPlatform.Services.Data.Interfaces;
    using RestaurantsPlatform.Web.Controllers;
    using RestaurantsPlatform.Web.ViewModels;
    using RestaurantsPlatform.Web.ViewModels.Comments;

    using static RestaurantsPlatform.Common.StringExtensions;
    using static RestaurantsPlatform.Web.Common.ErrorConstants;
    using static RestaurantsPlatform.Web.Common.NotificationsMessagesContants;

    [Area("Restaurants")]
    [Authorize]
    public class CommentsController : BaseController
    {
        private readonly ICommentService commentService;

        public CommentsController(ICommentService commentService)
        {
            this.commentService = commentService;
        }

        public async Task<IActionResult> AddCommentToRestaurant(CreateCommentInputModel input)
        {
            if (!this.ModelState.IsValid)
            {
                this.TempData[ErrorNotification] = WrontInput;
                return this.RedirectToRoute("restaurant", new { id = input.Id, name = input.RestaurantName.ToSlug() });
            }

            string userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var id = await this.commentService.AddCommentToRestaurantAsync(input.Id, input.CommentContent, userId);

            if (id == 0)
            {
                this.TempData[ErrorNotification] = CommentNotFound;
                return this.View(ErrorViewName, new ErrorViewModel
                {
                    RequestId = Activity.Current?.Id ?? this.HttpContext.TraceIdentifier,
                });
            }

            this.TempData[SuccessNotification] = string.Format(SuccessfullyCommentRestaurant, input.RestaurantName);
            return this.RedirectToRoute("restaurant", new { id = input.Id, name = input.RestaurantName.ToSlug() });
        }

        public async Task<IActionResult> Delete(DeleteCommentInputModel input)
        {
            var id = await this.commentService.DeleteCommentFromRestaurantAsync(input.CommentId, input.Id);

            if (id == null)
            {
                this.TempData[ErrorNotification] = CommentNotFound;
                return this.View(ErrorViewName, new ErrorViewModel
                {
                    RequestId = Activity.Current?.Id ?? this.HttpContext.TraceIdentifier,
                    Message = CommentNotFound,
                    StatusCode = 404,
                });
            }

            this.TempData[SuccessNotification] = SuccessfullyDeletedCommentFromRestaurant;
            return this.RedirectToRoute("restaurant", new { id = input.Id, name = input.Name });
        }

        [HttpPost]
        public async Task<IActionResult> Update([FromBody]UpdateCommentInputView input)
        {
            if (!this.ModelState.IsValid)
            {
                return this.BadRequest();
            }

            var updatedCommentId = await this.commentService.UpdateCommentAsync(input.CommentId, input.Content);

            if (updatedCommentId == 0)
            {
                return this.NotFound();
            }

            return this.Json(new { content = input.Content });
        }

        // public IActionResult LatestComments(int id)
        // {
        //    var comments = this.commentService.GetLatestComments<DetailsCommentViewModel>(id);

        // return this.Json(new { comments });
        // }

        // public ActionResult<IEnumerable<DetailsCommentViewModel>> MostPopularComments(int id)
        // {
        //    var comments = this.commentService.GetMostPopularComments<DetailsCommentViewModel>(id);

        // return new List<DetailsCommentViewModel>(comments);
        // }
    }
}
