import React, { Component, PropTypes } from 'react'
import { reduxForm } from 'redux-form'
import { registerValidate as validate } from './validation'
import { connect } from 'react-redux'
import * as actions from './actions'
import Helmet from 'react-helmet'
import { selectForm as select } from '../../../client/forms/reducers'
import ValidationSummary from '../../../client/forms/ValidationSummary'

class Register extends Component {
  submit(formInput) {
    return new Promise((resolve, reject) => {
      this.props.submitRegister(formInput)
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
      fields: { emailOrPhone },
      submitRegister, handleSubmit, submitting
    } = this.props

    return(
      <div>
        <Helmet title="Register" />
        <h2>Register.</h2>
        <form method="post" className="form-horizontal" role="form" onSubmit={handleSubmit(this.submit.bind(this))}>
          <h4>Create a new account.</h4>
          <hr />
          <div className="form-group">
            <div className="col-md-4">
              <label className="control-label sr-only">Email address or phone number</label>
              <input type="text" name="emailOrPhone" className="form-control" placeholder="Email address or phone number" disabled={submitting} {...emailOrPhone} />
            </div>
          </div>
          <div className="form-group">
            <div className="col-md-10">
              <button type="submit" className="btn btn-default" disabled={submitting} onClick={handleSubmit(this.submit.bind(this))}>Register</button>
            </div>
          </div>
          { emailOrPhone.touched && <ValidationSummary form={this.props} /> }
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

const form = 'register'
const fields = ['emailOrPhone']
const ReduxForm = reduxForm({
  form,
  fields,
  validate
})(connect(select, actions)(Register))

export default class Container extends Component {
  render() {
    return (
      <ReduxForm formKey={form} />
    )
  }
}
