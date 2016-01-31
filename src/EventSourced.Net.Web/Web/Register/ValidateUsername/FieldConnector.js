import React, { Component, PropTypes } from 'react'
import { connect } from 'react-redux'
import validate, { messages } from './validation'
import { blur, startAsyncValidation, stopAsyncValidation } from '../../Shared/actions/reduxForm'
import onSubmit from './actions'
import Field from './FieldComponent'
import _ from 'lodash'
import classNames from 'classnames'

class Connector extends Component {
  static propTypes = {
    form: PropTypes.object.isRequired,
    field: PropTypes.object.isRequired,
  };

  handleSubmit = () => {
    const { dispatch, field, form: { form, formKey, values, }, } = this.props
    dispatch(blur(form, field.name, field.value, true, formKey))
    var errors = validate(values)
    if (errors[field.name])
      return dispatch(stopAsyncValidation(form, errors, formKey))

    dispatch(startAsyncValidation(form, field.name, formKey))
    return onSubmit(values, dispatch)
      .catch(errors => (errors))
      .then(errors => (dispatch(stopAsyncValidation(form, errors, formKey))))
  };

  render() {
    const { handleSubmit, props: { field, submittedValue, form: { errors, asyncValidating, submitting, }, }, } = this
    const hasError = !!(errors[field.name] && field.touched && !asyncValidating)
    const hasSuccess = !!(field.value && submittedValue
      && field.value.toLowerCase() === submittedValue.toLowerCase()
      && !asyncValidating && !hasError)
    const className = classNames({
      'form-group': true,
      'has-error': hasError,
      'has-success': hasSuccess,
    })
    return(
      <Field
        className={className}
        asyncValidating={asyncValidating}
        submitting={submitting}
        hasError={hasError}
        hasSuccess={hasSuccess}
        field={field}
        error={errors[field.name]}
        success={messages.username.success}
        handleSubmit={handleSubmit}
      />
    )
  }
}

const select = (state, props) => {
  const apiCalls =  _.get(state, 'app.register.validateUsername.apiCalls', [])
  const lastApiCall = apiCalls.length > 0 ?
    state.app.register.validateUsername.apiCalls[0] : {}
  const { sent, } = lastApiCall
  return {
    submittedValue: _.get(sent, 'formInput.username', undefined),
  }
}

export default connect(select)(Connector)
