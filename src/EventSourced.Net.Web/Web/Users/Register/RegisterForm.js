import React, { Component, PropTypes } from 'react'
import { reduxForm } from 'redux-form';
import { connect } from 'react-redux'
import Helmet from 'react-helmet'
import * as actions from './actions'
import ValidationSummary from '../../components/ValidationSummary'
import { messages as validationMessages } from './validation'

const fields = ['emailOrPhone']

const validate = values => {
  const errors = { }
  if (!values.emailOrPhone) errors.emailOrPhone = validationMessages.emailOrPhone.empty
  return errors;
}

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
    } = this.props;

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
          <div className="text-danger form-errors">
            <ul></ul>
          </div>
          { emailOrPhone.touched && <ValidationSummary form={this.props} /> }
        </form>
      </div>
    )
  }

  static get propTypes() {
    return {
      fields: PropTypes.object.isRequired,
      handleSubmit: PropTypes.func.isRequired,
      submitting: PropTypes.bool.isRequired,
      serverErrors: PropTypes.object
    }
  }
}

function select(state) {
  return {
    submitting: state.app.ui.register.submitting || false,
    serverErrors: state.app.ui.register.serverErrors
  };
}

export default reduxForm({
  form: 'register',
  fields,
  validate
})(connect(select, actions)(Register))
