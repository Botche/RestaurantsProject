﻿namespace RestaurantsPlatform.Web.Areas.Restaurants.Controllers
{
    using System.Collections.Generic;
    using System.Security.Claims;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using RestaurantsPlatform.Services.Data.Interfaces;
    using RestaurantsPlatform.Web.Controllers;
    using RestaurantsPlatform.Web.ViewModels.Comments;
    using RestaurantsPlatform.Web.ViewModels.Restaurants;

    using static RestaurantsPlatform.Common.StringExtensions;
    using static RestaurantsPlatform.Web.Common.ErrorConstants;
    using static RestaurantsPlatform.Web.Common.NotificationsMessagesContants;

    [Area("Restaurants")]
    [Authorize]
    public class CommentsController : BaseController
    {
        private readonly ICommentService commentService;
        private readonly IRestaurantService restaurantService;

        public CommentsController(
            ICommentService commentService,
            IRestaurantService restaurantService)
        {
            this.commentService = commentService;
            this.restaurantService = restaurantService;
        }

        public async Task<IActionResult> AddCommentToRestaurant(CreateCommentInputModel input)
        {
            if (!this.ModelState.IsValid)
            {
                foreach (var modelState in this.ModelState.Values)
                {
                    foreach (var error in modelState.Errors)
                    {
                        this.TempData[ErrorNotification] = error.ErrorMessage;
                    }
                }

                return this.RedirectToRoute("restaurant", new { id = input.Id, name = input.RestaurantName.ToSlug() });
            }

            string userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var id = await this.commentService.AddCommentToRestaurantAsync(input.Id, input.CommentContent, userId);

            var result = this.CheckIfIdIsZero(id);
            if (result != null)
            {
                this.TempData[ErrorNotification] = CommentNotFound;
                return result;
            }

            this.TempData[SuccessNotification] = string.Format(SuccessfullyCommentRestaurant, input.RestaurantName);
            return this.RedirectToRoute("restaurant", new { id = input.Id, name = input.RestaurantName.ToSlug() });
        }

        public async Task<IActionResult> Delete(DeleteCommentInputModel input)
        {
            var id = await this.commentService.DeleteCommentFromRestaurantAsync(input.CommentId, input.Id);

            var result = this.CheckIfValueIsNull(id, CommentNotFound, 404);
            if (result != null)
            {
                this.TempData[ErrorNotification] = CommentNotFound;
                return result;
            }

            this.TempData[SuccessNotification] = SuccessfullyDeletedCommentFromRestaurant;
            return this.RedirectToRoute("restaurant", new { id = input.Id, name = input.Name });
        }

        [HttpPost]
        public async Task<IActionResult> Update([FromBody]UpdateCommentInputView input)
        {
            if (!this.ModelState.IsValid)
            {
                foreach (var modelState in this.ModelState.Values)
                {
                    foreach (var error in modelState.Errors)
                    {
                        this.TempData[ErrorNotification] = error.ErrorMessage;
                    }
                }

                return this.BadRequest();
            }

            var updatedCommentId = await this.commentService.UpdateCommentAsync(input.CommentId, input.Content);

            if (updatedCommentId == 0)
            {
                return this.NotFound();
            }

            return this.Json(new { content = input.Content });
        }

        [AllowAnonymous]
        public async Task<IActionResult> LatestComments(int restaurantId, string restaurantName)
        {
            var restaurant = await this.restaurantService.GetRestaurantByIdAndNameWithRecentComments(restaurantId, restaurantName);

            return this.PartialView("./PartialViews/_CommentsPartialView", restaurant);
        }

        [AllowAnonymous]
        public async Task<IActionResult> MostPopularComments(int restaurantId, string restaurantName)
        {
            var restaurant = await this.restaurantService.GetRestaurantByIdAndNameWithMostPopularComments(restaurantId, restaurantName);

            return this.PartialView("./PartialViews/_CommentsPartialView", restaurant);
        }
    }
}
