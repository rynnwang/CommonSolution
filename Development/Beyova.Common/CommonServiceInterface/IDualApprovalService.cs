//using System;
//using System.Collections.Generic;
//using Beyova;
//using Beyova.Api;
//using Beyova.RestApi;

//namespace Beyova.CommonServiceInterface
//{
//    [TokenRequired]
//    public interface IDualApprovalService
//    {
//        #region Dual Approval Rule

//        [ApiOperation("DualApprovalRule", HttpConstants.HttpMethod.Put)]
//        [ApiPermission(CommonServiceConstants.Permission.Administrator, ApiPermission.Required)]
//        DualApprovalRule CreateOrUpdateDualApprovalRule(DualApprovalRule rule);

//        [ApiOperation("DualApprovalRule", HttpConstants.HttpMethod.Post)]
//        [ApiPermission(CommonServiceConstants.Permission.Administrator, ApiPermission.Required)]
//        List<DualApprovalRule> QueryDualApprovalRule(DualApprovalRuleCriteria criteria);

//        #endregion

//        [ApiOperation("DualApproval", HttpConstants.HttpMethod.Put)]
//        DualApprovalRequest RequestDualApproval(DualApprovalRequest request);

//        [ApiOperation("DualApproval", HttpConstants.HttpMethod.Post, "Query")]
//        DualApprovalRequest GetDualApproval();

//        [ApiOperation("DualApproval", HttpConstants.HttpMethod.Post,"Approval")]
//        DualApprovalRequest TryDualApproval();
//    }
//}
