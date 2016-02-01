import { connect } from 'react-redux'
import validate, { messages } from './validation'
import { blur, startAsyncValidation, stopAsyncValidation } from '../../Shared/actions/reduxForm'
import onSubmit, { VALIDATE_USERNAME } from './actions'
import { createAction } from 'redux-actions'
import Field from './FieldComponent'
import _ from 'lodash'
import classNames from 'classnames'

class Connector extends React.Component {
  static propTypes = {
    submittedValue: React.PropTypes.string,
    touched: React.PropTypes.bool.isRequired,
    form: React.PropTypes.object.isRequired,
    field: React.PropTypes.object.isRequired,
  };

  handleSubmit = () => {
    const { dispatch, field, form: { formName, formKey, }, } = this.props
    const formInput = { username: field.value }
    dispatch(createAction(VALIDATE_USERNAME.TOUCHED)())
    dispatch(blur(formName, field.name, field.value, true, formKey))
    var errors = validate(formInput)
    if (errors[field.name])
      return dispatch(stopAsyncValidation(formName, errors, formKey))

    dispatch(startAsyncValidation(formName, field.name, formKey))
    return onSubmit(formInput, dispatch)
      .catch(errors => (errors))
      .then(errors => (dispatch(stopAsyncValidation(formName, errors, formKey))))
  };

  render() {
    const { handleSubmit, props: { field, touched, submittedValue, form: { errors, asyncValidating, submitting, }, }, } = this
    const hasError = !!(errors[field.name] && touched && !asyncValidating)
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
  const { sent, done, fail, } = lastApiCall
  return {
    submittedValue: _.get(sent, 'formInput.username', undefined),
    touched: _.get(state, 'app.register.validateUsername.touched', false),
  }
}

export default connect(select)(Connector)
