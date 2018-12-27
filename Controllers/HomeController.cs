using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MyVpubWallet.Node;
using Newtonsoft.Json;
using SocialCryptoBots;
using Tweetinvi;
using Tweetinvi.Credentials.Models;
using Tweetinvi.Models;

namespace MyVpubWallet.Controllers
{
	public class HomeController : Controller
	{
		public HomeController(VpubNode node) => this.node = node;
		private readonly VpubNode node;

		public async Task<IActionResult> Index()
		{
			await UpdateVpubPrice();
			return View();
		}

		private async Task UpdateVpubPrice()
		{
			var ticker = await CurrencyExtensions.GetTicker(Currency.mVP);
			ViewData["UsdRate"] = ticker.price_usd;
			ViewData["EurRate"] = ticker.price_eur;
			ViewData["BtcRate"] = ticker.price_btc;
			ViewData["VpubPrices"] = "$" + ticker.price_usd.ToString("#0.00") + " = €" +
				ticker.price_eur.ToString("#0.00");
			ViewData["MilliVpubPrices"] = "$" + (ticker.price_usd / 1000.0m).ToString("#0.00") +
				" = €" + (ticker.price_eur / 1000.0m).ToString("#0.00");
			UpdateCachedMixingPoolAmounts();
			ViewData["VpubMixingPoolAmount"] = vpubMixingPoolAmount.ToString("#0.#");
			ViewData["VpubMixingPoolPremixed"] = vpubMixingPoolPremixed.ToString("#0.#");
		}

		private void UpdateCachedMixingPoolAmounts()
		{
			if (DateTime.UtcNow.AddMinutes(-5) < lastTimeCheckedVpubMixingPool)
				return;
			lastTimeCheckedVpubMixingPool = DateTime.UtcNow;
			vpubMixingPoolAmount = 0m;
			vpubMixingPoolPremixed = 0m;
			try
			{
				foreach (var output in node.GetUnspentVpubOutputs())
				{
					vpubMixingPoolAmount += output.Amount * 1000m;
					if (output.Ps_Rounds >= 2)
						vpubMixingPoolPremixed += output.Amount * 1000m;
				}
			}
			catch
			{
				// ignored, if this crashes we use 0 for the mixing pool display
			}
		}

		private DateTime lastTimeCheckedVpubMixingPool;
		private decimal vpubMixingPoolAmount;
		private decimal vpubMixingPoolPremixed;

		public async Task<IActionResult> Swap()
		{
			await UpdateVpubPrice();
			return View();
		}

		public async Task<IActionResult> Address()
		{
			await UpdateVpubPrice();
			return View();
		}

		public async Task<IActionResult> Transaction()
		{
			await UpdateVpubPrice();
			return View();
		}

		public async Task<IActionResult> Tipping()
		{
			await UpdateVpubPrice();
			return View();
		}
		
		public async Task<IActionResult> Mixing()
		{
			await UpdateVpubPrice();
			return View();
		}

		public async Task<IActionResult> About()
		{
			await UpdateVpubPrice();
			return View();
		}

		public async Task<IActionResult> AboutCreateNewWallet()
		{
			await UpdateVpubPrice();
			return View();
		}

		public async Task<IActionResult> AboutHardwareWallets()
		{
			await UpdateVpubPrice();
			return View();
		}

		public async Task<IActionResult> AboutLedgerHardwareWallet()
		{
			await UpdateVpubPrice();
			return View();
		}

		public async Task<IActionResult> AboutTrezorHardwareWallet()
		{
			await UpdateVpubPrice();
			return View();
		}

		public async Task<IActionResult> AboutMyVpubWallet()
		{
			await UpdateVpubPrice();
			return View();
		}

		public async Task<IActionResult> AboutTransactionFees()
		{
			await UpdateVpubPrice();
			return View();
		}

		public async Task<IActionResult> AboutInstantSend()
		{
			await UpdateVpubPrice();
			return View();
		}

		public async Task<IActionResult> AboutPrivateSend()
		{
			await UpdateVpubPrice();
			return View();
		}

		public async Task<IActionResult> AboutVideos()
		{
			await UpdateVpubPrice();
			return View();
		}

#if DEBUG
		public IActionResult TestLedger() => View();
		public IActionResult TestTrezor() => View();
		public IActionResult ScanQR() => View();
#endif
		public async Task<IActionResult> Error()
		{
			await UpdateVpubPrice();
			ViewData["RequestId"] = Activity.Current?.Id ?? HttpContext.TraceIdentifier;
			return View();
		}

		public IActionResult Redeem()
		{
			var redirectUrl = "//MyVpubWallet.org/RedeemRedirect";
			var context = AuthFlow.InitAuthentication(TwitterConsumerCredentials, redirectUrl);
			TwitterAuthKey = context.Token.AuthorizationKey;
			TwitterAuthSecret = context.Token.AuthorizationSecret;
			return Redirect(context.AuthorizationURL);
		}

		public static string TwitterAuthKey;
		public static string TwitterAuthSecret;
		public static IConsumerCredentials TwitterConsumerCredentials
			=> new ConsumerCredentials("QvLbPlqr0B78lNVPZGU5zMCI0",
				"LWdDRdxaHjxmJ3wOtXyWHsTB5ZgG6ghmeIY240Ak1H21tyQ28A");

		public IActionResult RedeemRedirect()
		{
			try
			{
				var token = new AuthenticationToken
				{
					AuthorizationKey = TwitterAuthKey,
					AuthorizationSecret = TwitterAuthSecret,
					ConsumerCredentials = TwitterConsumerCredentials
				};
				var verifierCode = Request.Query["oauth_verifier"];
				var userCreds = AuthFlow.CreateCredentialsFromVerifierCode(verifierCode, token);
				var user = Tweetinvi.User.GetAuthenticatedUser(userCreds);
				ViewData["Result"] = "Failed to authenticate: No redeem link stored for @" + user.ScreenName;
			}
			catch (Exception ex)
			{
				ViewData["Result"] = "Failed to authenticate: " +
#if DEBUG
					ex;
#else
					ex;//.Message;
#endif
			}
			return View();
		}

		[HttpGet]
		public IActionResult GenerateRawTx(string utxos, string channel, decimal amount, string sendTo,
			decimal remainingAmount, string remainingAddress, bool instantSend, bool privateSend,
			string extraText)
		{
			try
			{
				node.UserIp = HttpContext.Connection.RemoteIpAddress.ToString();
				return Json(node.TryGenerateRawTx(utxos, amount, sendTo, remainingAmount, remainingAddress,
					instantSend, privateSend));
			}
			catch (Exception ex)
			{
				return GetErrorResult(ex, "Failed to generate raw tx");
			}
		}

		private IActionResult GetErrorResult(Exception ex, string errorMessage)
		{
			if (ex.Message == "No information available about transaction")
				return StatusCode(500,
					"Unable to generate raw tx, inputs are not found in the Vpub blockchain " +
					"(are you on testnet or have an invalid address or transaction id?)");
			if (ex.Message.Contains(
				"16: mandatory-script-verify-flag-failed (Script failed an OP_EQUALVERIFY operation)"))
				errorMessage += ": Wrong inputs were signed (used address index is already spent), " +
					"unable to confirm signed transaction. Double spends are not allowed!<br/>Details";
			errorMessage += ": " +
#if DEBUG
				ex.ToString().Replace("MyVpubWallet.Controllers.HomeController+", "").
					Replace("MyVpubWallet.Controllers.HomeController.", "");
#else
				ex.GetType().Name + ": " + ex.Message;
#endif
			if (node.AdditionalErrorInformation != null)
				errorMessage += "<br/><br/>Additional Information: " +
					JsonConvert.SerializeObject(node.AdditionalErrorInformation);
			return StatusCode(500, errorMessage);
		}

		[HttpGet]
		public IActionResult SendSignedTx(string signedTx, bool instantSend)
		{
			try
			{
				node.UserIp = HttpContext.Connection.RemoteIpAddress.ToString();
				var txId = node.BroadcastSignedTxIntoVpubNetwork(signedTx, instantSend);
				return Ok(txId);
			}
			catch (Exception ex)
			{
				return GetErrorResult(ex, "Failed to send signed tx");
			}
		}

		[HttpGet]
		public IActionResult GeneratePrivateSendAddress(string toAddress)
		{
			try
			{
				node.UserIp = HttpContext.Connection.RemoteIpAddress.ToString();
				return Ok(node.GenerateNewAddress("0|" + toAddress, true));
			}
			catch (Exception ex)
			{
				return GetErrorResult(ex, "Failed to generate PrivateSend Address");
			}
		}
	}
}