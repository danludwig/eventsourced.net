import React, { Component, PropTypes } from 'react'
import { reduxForm } from 'redux-form';
import validate from './validation'
import { connect } from 'react-redux'
import { selectForm as select } from '../../forms/reducers'
import * as actions from './actions'
import Helmet from 'react-helmet'
import ValidationSummary from '../../forms/ValidationSummary'

class Login extends Component {
  submit(formInput) {
    return new Promise((resolve, reject) => {
      this.props.submitLogin(formInput)
        .then(() => {
          if (this.props.serverErrors) {
            return reject(this.props.serverErrors)
          }
          return resolve()
        })
    })
  }

  render() {
    const {
      fields: { login, password },
      submitLogin, handleSubmit, submitting
    } = this.props;
    return (
      <div>
        <Helmet title="Log in" />
        <h2>Log in.</h2>
        <form id="login_form" action="/api/login" method="post" className="form-horizontal" role="form" onSubmit={handleSubmit(this.submit.bind(this))}>
          <h4>Use a local account to log in.</h4>
          <hr />
          <div className="form-group">
            <div className="col-md-4">
              <label className="control-label sr-only">Email address, phone number, or username</label>
              <input type="text" className="form-control" placeholder="Email address, phone number, or username" disabled={submitting} {...login} />
            </div>
          </div>
          <div className="form-group">
            <div className="col-md-4">
              <label className="control-label sr-only">Password</label>
              <input type="password" className="form-control" placeholder="Password" disabled={submitting} {...password} />
            </div>
          </div>
          <div className="form-group">
            <div className="col-md-10">
              <button type="submit" className="btn btn-default" disabled={submitting} onClick={handleSubmit(this.submit.bind(this))}>Log in</button>
            </div>
          </div>
          { login.touched && password.touched && <ValidationSummary form={this.props} /> }
        </form>
      </div>
    )
  }

  static get propTypes() {
    return {
      submitting: PropTypes.bool.isRequired,
      serverErrors: PropTypes.object
    }
  }
}

const form = 'login'
const fields = ['login', 'password']
const ReduxForm = reduxForm({
  form,
  fields,
  validate
})(connect(select, actions)(Login))

export default class Container extends Component {
  render() {
    return (
      <ReduxForm formKey={form} />
    )
  }
}
