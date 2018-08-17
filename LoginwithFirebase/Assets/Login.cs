using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Firebase.Auth;
using Facebook.Unity;
using UnityEngine.SceneManagement;

public class Login : MonoBehaviour {

	//public Text userIdText;
	private FirebaseAuth mAuth;
	

	private void Awake()
	{
		
	 Firebase.Auth.FirebaseAuth auth = Firebase.Auth.FirebaseAuth.DefaultInstance;
			
		if (!FB.IsInitialized)
		{
			FB.Init();
		}
		else
		{
			FB.ActivateApp();
            
        }
	}

	public void LogIn()
	{
		FB.LogInWithReadPermissions(callback:OnLogIn);


	}
	private void OnLogIn(ILoginResult result)
	{
		if (FB.IsLoggedIn)
		{
			AccessToken tocken = AccessToken.CurrentAccessToken;
			Debug.Log("login successful");
			//userIdText.text = tocken.UserId;
			Credential credential = FacebookAuthProvider.GetCredential(tocken.TokenString);
			accessToken (credential);
            SceneManager.LoadScene("AfterLogin");

            Debug.Log("Change of Scence");

        }
		else
		{
			Debug.Log("Login Failed");
		}
	}

	public void accessToken(Credential firebaseResult)
	{
		FirebaseAuth auth = FirebaseAuth.DefaultInstance;
		if (!FB.IsLoggedIn)
		{
			return;
		}

		auth.SignInWithCredentialAsync(firebaseResult).ContinueWith(task =>
			{
				if (task.IsCanceled)
				{
					Debug.LogError("SignInWithCredentialAsync was canceled.");
					return;
				}
				if (task.IsFaulted)
				{
					Debug.LogError("SignInWithCredentialAsync encountered an error: " + task.Exception);
					return;
				}

				FirebaseUser newUser = task.Result;
				Debug.LogFormat("User signed in successfully: {0} ({1})",
					newUser.DisplayName, newUser.UserId);
        });
	}
}

