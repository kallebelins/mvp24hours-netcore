//=====================================================================================
// Developed by Kallebe Lins (https://github.com/kallebelins)
//=====================================================================================
// Reproduction or sharing is free! Contribute to a better world!
//=====================================================================================
using FluentValidation;
using Mvp24Hours.Core.Contract.Infrastructure.Contexts;
using Mvp24Hours.Core.ValueObjects.Infrastructure;
using Mvp24Hours.Helpers;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Mvp24Hours.Extensions
{
    public static class ValidatorEntityExtensions
    {
        public static bool Validate<TEntity>(this TEntity entity, INotificationContext _context = null, IValidator<TEntity> _validator = null)
            where TEntity : class
        {
            var notifyCntxt = _context ?? ServiceProviderHelper.GetService<INotificationContext>();
            var validator = _validator ?? ServiceProviderHelper.GetService<IValidator<TEntity>>();
            if (validator != null)
            {
                var validationResult = validator.Validate(entity);
                if (!validationResult.IsValid)
                {
                    if (notifyCntxt != null)
                    {
                        var notifications = validationResult.Errors
                            .Select(x => new Notification(x.ErrorCode, x.ErrorMessage, Core.Enums.MessageType.Error))
                            .ToList();

                        notifyCntxt.Add(notifications);
                    }
                    return false;
                }
            }
            else
            {
                var validationRslts = new List<ValidationResult>();
                var validationCntxt = new ValidationContext(entity, null, null);
                if (!Validator.TryValidateObject(entity, validationCntxt, validationRslts, true))
                {
                    if (notifyCntxt != null)
                    {
                        foreach (var item in validationRslts)
                        {
                            notifyCntxt.Add(string.Join("|", item.MemberNames), item.ErrorMessage, Core.Enums.MessageType.Error);
                        }
                    }
                    return false;
                }
            }
            return true;
        }
    }
}
