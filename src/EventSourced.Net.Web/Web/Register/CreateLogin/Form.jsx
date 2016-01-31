import { reduxForm } from 'redux-form'
import ValidateUsernameField from '../ValidateUsername/FieldConnector'
import validate, { messages } from './validation'
import ValidationSummary from '../../Shared/ValidationSummary'

const form = 'redeem'
const fields = ['username', 'password', 'passwordConfirmation', 'correlationId', 'token']
const reduxFormConfig = {
  form,
  fields,
  validate,
}

class Form extends React.Component {
  static propTypes = {
    viewData: React.PropTypes.object.isRequired,
    fields: React.PropTypes.object.isRequired,
    handleSubmit: React.PropTypes.func.isRequired,
    submitting: React.PropTypes.bool.isRequired,
  };

  render() {
    const { fields: { username, password, passwordConfirmation, token, },
      viewData, handleSubmit, asyncValidating, submitting, submitFailed } = this.props
    return(
      <form method="post" className="form-horizontal" role="form" onSubmit={handleSubmit}>

        <div className="form-group has-success">
        { viewData.purpose === 'CreateUserFromEmail' ?
          <div className="col-md-6">
            <label className="control-label sr-only">Email address:</label>
            <input type="email" name="emailOrPhone" className="form-control" value={viewData.contactValue} disabled="disabled" />
            <p className="help-block">You will be able to login using your email address above.</p>
          </div>
          :
          <div className="col-md-6">
            <label className="control-label sr-only">Phone number:</label>
            <input type="tel" name="emailOrPhone" className="form-control" value={viewData.phoneNumberFormatted} disabled="disabled" />
            <p className="help-block">You will be able to login using your phone number above.</p>
          </div>
          }
        </div>

        <ValidateUsernameField form={{ form, ...this.props, }} field={username} />

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
            <button type="submit" className="btn btn-default" disabled={submitting || asyncValidating}>Create login</button>
          </div>
        </div>

        { submitFailed &&
          <ValidationSummary form={this.props} /> }
      </form>
    )
  }
}

export default reduxForm(reduxFormConfig)(Form)
