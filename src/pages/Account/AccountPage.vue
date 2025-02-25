<script>
import {ensureLoggedIn} from "@/assets/js/utils.js";
import LanguageDropdown from "@/components/LanguageDropdown.vue";

export default {
  components: {LanguageDropdown},
  setup() {
    const user = ensureLoggedIn();
    return {
      user,
      errors: []
    };
  }
};
</script>

<template>
  <div class="row">
    <div class="col-md-3 border-right">
      <div class="d-flex flex-column align-items-center text-center p-3 py-5"><img class="rounded-circle mt-5" width="150px" src="https://st3.depositphotos.com/15648834/17930/v/600/depositphotos_179308454-stock-illustration-unknown-person-silhouette-glasses-profile.jpg" alt="Generic profile picture">
        <span class="font-weight-bold">{{ user.username }}</span><span> </span></div>
    </div>
    <div class="col-md-5 border-right">
      <div class="p-3 py-5">
        <div class="d-flex justify-content-between align-items-center mb-3">
          <h4 class="text-right">{{ $t('profile-settings') }}</h4>
        </div>
        <div class="row mt-2">
          <div class="col-md-6"><label class="labels">{{ $t('username') }}</label><input style="background-color: rgb(34, 34, 34); color: #ffffff" type="text" class="form-control" placeholder="{{ $t('username') }}" value="{{ user.username }}" id="username"></div>
          @if (_errors.Contains(Error.UsernameInvalid)) {
          <p style="color: red; font-size: 13px">{{ $t('invalid-username') }}</p>
          }
          else {
          if (_errors.Contains(Error.UsernameTaken)) {
          <p style="color: red; font-size: 13px">@Localiser["username-taken"]</p>
          }
          }
        </div>
        <div class="row mt-3">
          <div class="col-md-12">
            <label class="labels">
              @Localiser["email"] @((string.IsNullOrWhiteSpace(_user.Email) ?
              "" :
              _verifiedEmail ?
              $"<span class='text-success'>({Localiser["verified"]})</span>" :
              $"<span class='text-warning'>({Localiser["not-verified"]})</span>").MarkupString())
            </label><input style="background-color: rgb(34, 34, 34); color: #ffffff" type="text" class="form-control" placeholder="{{ $t('email') }}" value="@_user.Email" id="email">
          </div>
          @if (_errors.Contains(Error.EmailInvalid)) {
          <p style="color: red; font-size: 13px">@Localiser["invalid-email"]</p>
          }

          <div class="row mt-2">
            <div class="col-md-6">
              <label class="labels">@Localiser["language"]</label>
              <LanguageDropdown DefaultValue="@_user.Language" Placeholder="Choose Language" Class="form-control text-white" Style="background-color: rgb(34, 34, 34); color: #ffffff" Id="language"></LanguageDropdown>
            </div>
          </div>

          <div style="padding-top: 20px"></div>
          <hr/>

          <h4>@Localiser["change-password"]</h4>
          <div class="col-md-12"><label class="labels">@Localiser["password"]</label><input style="background-color: rgb(34, 34, 34); color: #ffffff" id="password" type="password" class="form-control" placeholder="***********" value=""></div>
          <div class="col-md-12"><label class="labels">@Localiser["confirm-password"]</label><input style="background-color: rgb(34, 34, 34); color: #ffffff" id="confirmPassword" type="password" class="form-control" placeholder="***********" value=""></div>
          @if (_errors.Contains(Error.PasswordDifferent)) {
          <p style="color: red; font-size: 13px">@Localiser["passwords-dont-match"]</p>
          }
          <div style="padding-top: 20px"></div>

          <h4>@Localiser["security"]</h4>
          @if (_user.TotpEnabled) {
          <div class="row mt-3">
            <button @onclick="Disable2Fa" class="btn btn-danger">@Localiser["disable-2fa"]</button>
            <div style="padding-top: 10px"></div>
            <button @onclick="SetupTotp" class="btn btn-primary">@Localiser["setup-totp-app"]</button>
          </div>
          }
          else {
          <button @onclick="SetupTotp" class="btn btn-success">@Localiser["setup-2fa"]</button>
          }

          <div style="padding-top: 20px"></div>
          <hr/>

        </div>

        <div class="row mt-3">
          <div class="col-md-6"><label class="labels">@Localiser["id"]</label><input type="text" style="background-color: rgb(34, 34, 34); color: #ffffff" class="form-control" placeholder="{{ $t('id') }}" value="@_user.Id" readonly></div>
          <div class="col-md-6"><label class="labels">@Localiser["account-type"]</label><input type="text" style="background-color: rgb(34, 34, 34); color: #ffffff" class="form-control" placeholder="Normal" value="@(((AccountAccessLevel) Enum.ToObject(typeof(AccountAccessLevel), _user.PermLevel ?? 1)).ToString())" readonly></div>
          <div class="col-md-6"><label class="labels">@Localiser["premium-level"]</label><input type="text" style="background-color: rgb(34, 34, 34); color: #ffffff" class="form-control" placeholder="Normal" value="@_premiumText" readonly></div>
        </div>
        <div class="mt-5 text-center"><button class="btn btn-primary profile-button" type="button" @onclick="Save">@Localiser["save-changes"]</button></div>
      </div>
    </div>
  </div>
</template>

<style scoped>

</style>