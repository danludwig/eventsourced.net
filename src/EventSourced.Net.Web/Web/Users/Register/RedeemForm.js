import React, { Component, PropTypes } from 'react'
import { reduxForm } from 'redux-form'
import { validateRedeem as validate } from './validation'
import { connect } from 'react-redux'
import * as actions from './actions'
import Helmet from 'react-helmet'
import { selectForm as select } from '../../Shared/selectors'
import ValidationSummary from '../../Shared/ValidationSummary'
import CheckUsernameField from './CheckUsernameField'

class Redeem extends Component {
  static propTypes = {
    params: PropTypes.object.isRequired,
    submitting: PropTypes.bool.isRequired,
    serverErrors: PropTypes.object
  };

  submit = (formInput, dispatch) => {
    console.log('submit method invoked')
    const { params: { correlationId, token }, submitCreateLogin } = this.props
    return new Promise((resolve, reject) => {
      return dispatch(submitCreateLogin(correlationId, token, formInput))
        .then(() => {
          if (this.props.serverErrors) {
            return reject(this.props.serverErrors)
          }
          return resolve()
        })
    })
  };

  render() {
    const { fields: { username, password, passwordConfirmation },
      submitRedeem, handleSubmit, submitting, dispatch } = this.props
    const displayNone = { display: 'none' }
    return(
      <div>
        <Helmet title="Create login" />
        <h2>Create login.</h2>
        <form method="post" className="form-horizontal" role="form" onSubmit={handleSubmit(this.submit)}>
          <h4>Choose a username and password.</h4>
          <hr />
          <div className="form-group has-success">
            { this.props.data.purpose === 'CreateUserFromEmail' ?
            <div className="col-md-6">
                <label className="control-label sr-only">Email address:</label>
                <input type="email" name="emailOrPhone" className="form-control" value={this.props.data.contactValue} disabled="disabled" />
                <p className="help-block">You will be able to login using your email address above.</p>
            </div>
            :
            <div className="col-md-6">
              <label className="control-label sr-only">Phone number:</label>
              <input type="tel" name="emailOrPhone" className="form-control" value={this.props.data.phoneNumberFormatted} disabled="disabled" />
              <p className="help-block">You will be able to login using your phone number above.</p>
            </div>
            }
          </div>
          <CheckUsernameField field={username} dispatch={dispatch} />
          <div className="form-group">
            <div className="col-md-6">
              <label className="control-label sr-only">Password</label>
              <input type="password" className="form-control" placeholder="Create a password" disabled={submitting} {...password} />
              <p className="help-block">Must be at least 8 characters long.</p>
            </div>
          </div>
          <div className="form-group">
            <div className="col-md-6">
              <label className="control-label sr-only">Confirm Password</label>
              <input type="password" className="form-control" placeholder="Enter same password as above" disabled={submitting} {...passwordConfirmation} />
              <p className="help-block">Make double sure you typed it correctly.</p>
            </div>
          </div>
          <div className="form-group">
            <div className="col-md-10">
              <input type="hidden" name="token" value={this.props.params.token} />
              <button type="submit" className="btn btn-default" disabled={submitting} onClick={handleSubmit(this.submit)}>Create login</button>
            </div>
          </div>
          { username.touched && password.touched && passwordConfirmation.touched && <ValidationSummary form={this.props} /> }
        </form>
      </div>
    )
  }
}

function customSelect(state, props) {
  let selection = select(state, props)
  if (selection.serverErrors) {
    const { _error, ...rest } = selection.serverErrors
    if (_error) selection.error = _error
    selection.errors = {
      ...props.errors,
      ...rest,
    }
  }
  return selection
}

const form = 'redeem'
const fields = ['username', 'password', 'passwordConfirmation']
const ReduxForm = reduxForm({
  form,
  fields,
  validate
})(connect(customSelect, actions)(Redeem))

export default class Container extends Component {
  render() {
    return (
      <ReduxForm formKey={form} params={{...this.props.params, ...this.props.location.query}} />
    )
  }
}
