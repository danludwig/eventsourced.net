import React, { Component, PropTypes } from 'react'
import { reduxForm } from 'redux-form'
import { validateRedeem as validate } from './validation'
import { connect } from 'react-redux'
import * as actions from './actions'
import Helmet from 'react-helmet'
import { selectForm as select } from '../../../client/forms/reducers'
import ValidationSummary from '../../../client/forms/ValidationSummary'

class Redeem extends Component {
  submit(formInput) {
    console.log('submit method invoked')
    // return new Promise((resolve, reject) => {
    //   this.props.submitRedeem(this.props.params.correlationId, formInput)
    //     .then(() => {
    //       if (this.props.serverErrors) {
    //         return reject(this.props.serverErrors)
    //       }
    //       return resolve()
    //     })
    // })
  }

  render() {
    const {
      fields: { username, password, passwordConfirmation },
      submitRedeem, handleSubmit, submitting
    } = this.props
    const displayNone = { display: 'none' }
    return(
      <div>
        <Helmet title="Create login" />
        <h2>Create login.</h2>
        <form method="post" className="form-horizontal" role="form" onSubmit={handleSubmit(this.submit.bind(this))}>
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
          <div className="form-group">
            <div className="col-md-6">
              <label className="control-label sr-only">Username</label>
              <div className="input-group">
                <input type="text" name="username" className="form-control" placeholder="Choose a username" disabled={submitting} {...username} />
                <span className="input-group-btn input-group-btn-right" style={{left: '1px'}}>
                  <button type="button" className="btn btn-default">
                    Check availability
                    {' '}
                    <span className="glyphicon glyphicon-search text-info" aria-hidden="true"></span>
                    <span className="glyphicon glyphicon-remove text-danger" aria-hidden="true" style={displayNone}></span>
                    <span className="glyphicon glyphicon-ok text-success" aria-hidden="true" style={displayNone}></span>
                  </button>
                </span>
              </div>
            </div>
            <div className="col-md-12">
              <p className="help-block help-info default">Use between 2 and 12 numbers, letters, hypens, underscores, and dots.</p>
              <p className="help-block help-info checking-availability" style={displayNone}>Checking availability...</p>
              <p className="help-block help-result" style={displayNone}></p>
            </div>
          </div>
          <div className="form-group">
            <div className="col-md-6">
              <label className="control-label sr-only">Password</label>
              <input type="password" name="password" className="form-control" placeholder="Create a password" disabled={submitting} {...password} />
              <p className="help-block">Must be at least 8 characters long.</p>
            </div>
          </div>
          <div className="form-group">
            <div className="col-md-6">
              <label className="control-label sr-only">Confirm Password</label>
              <input type="password" name="passwordConfirmation" className="form-control" placeholder="Enter same password as above" disabled={submitting} {...passwordConfirmation} />
              <p className="help-block">Make double sure you typed it correctly.</p>
            </div>
          </div>
          <div className="form-group">
            <div className="col-md-10">
              <input type="hidden" name="token" value={this.props.params.token} />
              <button type="submit" className="btn btn-default" disabled={submitting} onClick={handleSubmit(this.submit.bind(this))}>Create login</button>
            </div>
          </div>
          { username.touched && password.touched && passwordConfirmation.touched && <ValidationSummary form={this.props} /> }
        </form>
      </div>
    )
  }

  static get propTypes() {
    return {
      params: PropTypes.object.isRequired,
      submitting: PropTypes.bool.isRequired,
      serverErrors: PropTypes.object
    }
  }
}

const form = 'redeem'
const fields = ['username', 'password', 'passwordConfirmation']
const ReduxForm = reduxForm({
  form,
  fields,
  validate
})(connect(select, actions)(Redeem))

export default class Container extends Component {
  render() {
    return (
      <ReduxForm formKey={form} params={{...this.props.params, ...this.props.location.query}} />
    )
  }
}
